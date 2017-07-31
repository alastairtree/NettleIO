using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    class StagePerformer<TStage> : IStagePerformer
    {
        protected LambdaExpression stageExecutionExpression;

        public TStage StageInstance { get; private set; }

        private MethodCallExpression MethodCallExpression
        {
            get
            {
                var callExpression = stageExecutionExpression.Body as MethodCallExpression;
                if (callExpression == null)
                {
                    throw new ArgumentException("Expression body should be of type `MethodCallExpression`",
                        nameof(stageExecutionExpression));
                }
                return callExpression;
            }
        }

        protected MethodInfo Method => MethodCallExpression.Method;

        public IActionResult Result { get; protected set; }
        public object Value { get; protected set; }

        public virtual void PrepareToPerform(IActivator activator)
        {
            if (!Method.IsStatic)
            {
                StageInstance = activator.Create<TStage>();
                if (StageInstance == null)
                {
                    throw new InvalidOperationException(
                        $"activator returned NULL instance of the '{typeof(TStage)}' type.");
                }
            }
        }

        public virtual async Task Perform(params object[] arguments)
        {
            var task = Method.Invoke(StageInstance, arguments) as Task<IActionResult>;

            if (task == null)
            {
                Result = Core.Result.Fail(
                    $"Invoking {Method} returned null or unmatching type. Expected {typeof(Task<IActionResult>)}");
                return;
            }

            Result = await task;
        }

        public static StagePerformer<TStage> Build(
            Expression<Func<TStage, Task<IActionResult>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            return new StagePerformer<TStage> {stageExecutionExpression = stageExecutionExpression};
        }

        public static StagePerformer<TStage> Build<TInput>(
            Expression<Func<TStage, TInput, Task<IActionResult>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            return new StagePerformer<TStage> {stageExecutionExpression = stageExecutionExpression};
        }
    }

    class StagePerformer<TStage, TResult> : StagePerformer<TStage>
    {
        public static StagePerformer<TStage, TResult> Build(
            Expression<Func<TStage, Task<IValueResult<TResult>>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            return new StagePerformer<TStage, TResult> {stageExecutionExpression = stageExecutionExpression};
        }

        public static StagePerformer<TStage, TResult> Build<TInput>(
            Expression<Func<TStage, TInput, Task<IValueResult<TResult>>>> stageExecutionExpression)
        {
            if (stageExecutionExpression == null) throw new ArgumentNullException(nameof(stageExecutionExpression));

            return new StagePerformer<TStage, TResult> {stageExecutionExpression = stageExecutionExpression};
        }

        public override async Task Perform(params object[] arguments)
        {
            var task = Method.Invoke(StageInstance, arguments) as Task<IValueResult<TResult>>;

            if (task == null)
            {
                Result = Core.Result.Fail(
                    $"Invoking {Method} returned null or unmatching type. Expected {typeof(Task<IValueResult<TResult>>)}");
                return;
            }

            Result = await task;

            var result = (Result as IValueResult<TResult>);
            if (result != null)
                Value = result.Value;
        }
    }
}