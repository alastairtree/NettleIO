using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using NettleIO.Core.Exceptions;

namespace NettleIO.Core
{
    public class StagePerformer<TStage> : IStagePerformer<TStage>
    {
        public StagePerformer(LambdaExpression stageExecutionExpression)
        {
            this.stageExecutionExpression = stageExecutionExpression ?? throw new ArgumentNullException(nameof(stageExecutionExpression));
        }

        private readonly LambdaExpression stageExecutionExpression;

        public TStage StageInstance { get; private set; }

        private MethodCallExpression MethodCallExpression
        {
            get
            {
                var callExpression = stageExecutionExpression.Body as MethodCallExpression;
                if (callExpression == null)
                    throw new ArgumentException("Expression body should be of type `MethodCallExpression`",
                        nameof(stageExecutionExpression));
                return callExpression;
            }
        }

        protected MethodInfo Method => MethodCallExpression.Method;

        public IStageResult Result { get; protected set; }
        public object Value { get; protected set; }

        public virtual void PrepareToPerform(IActivator activator)
        {
            if (!Method.IsStatic)
            {
                StageInstance = activator.Create<TStage>();
                if (StageInstance == null)
                    throw new InvalidOperationException(
                        $"activator returned NULL instance of the '{typeof(TStage)}' type.");
            }
        }

        public virtual async Task Perform(params object[] arguments)
        {
            var task = Method.Invoke(StageInstance, arguments);

            if(task is null)
                throw new StageReturnsTheWrongTypeException(Method, null);


            if (!(task is Task<IStageResult>))
            {
                throw new StageReturnsTheWrongTypeException(Method, task.GetType());
            }

            Result = await (task as Task<IStageResult>);
        }
    }

    internal class StagePerformer<TStage, TResult> : StagePerformer<TStage>
    {
        public StagePerformer(LambdaExpression stageExecutionExpression) : base(stageExecutionExpression)
        {
        }

        public override async Task Perform(params object[] arguments)
        {
            var task = Method.Invoke(StageInstance, arguments);

            if (task is null)
                throw new StageReturnsTheWrongTypeException(Method, null);


            if (!(task is Task<IStageValueResult<TResult>>))
            {
                throw new StageReturnsTheWrongTypeException(Method, task.GetType());
            }

            var result = await (task as Task<IStageValueResult<TResult>>);
            Result = result;
            if (result != null)
                Value = result.Value;

        }
    }
}