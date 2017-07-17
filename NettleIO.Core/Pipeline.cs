using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NettleIO.Core
{
    public class Pipeline : IPipeline
    {
        private readonly IActivator activator;

        public Pipeline(IActivator activator)
        {
            this.activator = activator;
        }

        private Type sourceType;
        private List<Type> stagesTypes = new List<Type>();
        private List<Expression> expressions = new List<Expression>();
        private Type destinationType;

        public IPipeline RegisterSource<TSource>() where TSource : ISource
        {
            this.sourceType = typeof(TSource);
            return this;    
        }

        public IPipeline AddStage<TStage, TDataIn, TDataOut>() where TStage : IStage<TDataIn, TDataOut>
        {
            stagesTypes.Add(typeof(TStage));
            return this;
        }

        public IPipeline AddStage<TStage, TDataIn, TDataOut>(Expression<Func<TStage, TDataIn, Task<IValueResult<TDataOut>>>> executeExpression) where TStage : IStage<TDataIn, TDataOut>
        {
            expressions.Add(executeExpression);
            return AddStage<TStage, TDataIn, TDataOut>(); ;
        }

        public IPipeline AddStage<TStage, TData>() where TStage : IStage<TData>
        {
            return AddStage<TStage, TData, TData>();
        }

        public IPipeline RegisterDestination<TDestination, TData>() where TDestination : IDestination<TData>
        {
            destinationType = typeof(TDestination);
            return this;
        }

        public async Task Execute()
        {
            Validate();

            var source = activator.Create(sourceType);

            Func<ISource, Task<IValueResult<object>>> recieve = s => s.RecieveAsync();

            IValueResult<object> recieved = await recieve(source as ISource);
            object value = recieved.Value;
            
            if(value == null)
                throw new Exception("Empty source!");

            foreach (var stagesType in stagesTypes)
            {
                var stage = activator.Create(stagesType);
                Func<IStage, Task<IValueResult<object>>> stageFunc = s => s.Execute(value);
                IValueResult<object> stageResult = await stageFunc((IStage)stage);
                value = stageResult.Value;


                //ffs just do this?!
                // value = ((IStage) stage).Execute(value);
            }

            //for (int i = 0; i < stagesTypes.Count; i++)
            //{
            //    var stage = (IStage)activator.Create(stagesTypes[i]);
            //    var expression = expressions[i];
            //    var lambda =  Expression.Lambda<Func<IStage, object, Task<IValueResult<object>>>>(
            //        expression);

            //    value = lambda.Compile()(stage, value);
            //}

            var destination = (IDestination) activator.Create(destinationType);

            Func<IDestination, Task<IActionResult>> send = s => s.SendAsync(value);

            var completed = await send(destination);

            Console.WriteLine("Completed!");
        }

        //TODO: Build expressions for the pipeline?!

        //private static Func<object> CreateLambdaMethodCall(Type type, string methodName)
        //{
        //    var itemExpressionVariable = Expression.Variable(type);
        //    var method = type.GetRuntimeMethod()

        //    var propertyExpression = Expression.Property(itemExpressionVariable, fieldName);

        //    Expression propertyExpressionToObject = Expression.Convert(propertyExpression, typeof(object));
        //    var getter =
        //        Expression.Lambda<Func<T, object>>(propertyExpressionToObject, itemExpressionVariable).Compile();

        //    return getter;
        //}

        protected virtual void Validate()
        {
            Console.WriteLine("Doing some validation....?");
        }
    }
}