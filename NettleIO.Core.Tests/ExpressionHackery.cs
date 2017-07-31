using System;
using System.Linq;
using System.Linq.Expressions;
using NettleIO.Core.Tests;
using Xunit;

namespace NettleIO.Core.IgnoreTheseTests
{
    public class ExpressionHackery
    {
        public class A
        {
            public B MakeB()
            {
                return new B {Name = "Borris"};
            }
        }

        public class B
        {
            public string Name { get; set; }
        }

        public class C
        {
            public D MessWithB(B b)
            {
                return new D {FullName = b.Name.Replace("B", "D")};
            }

            public D ReverseDName(D d)
            {
                d.FullName = new string(d.FullName.Reverse().ToArray());
                return d;
            }
        }

        public class D
        {
            public string FullName { get; set; }
        }

        public class PipelineBuilder<TResult> : PipelineBuilder
        {
            private Expression<Func<IActivator, TResult>> createAndExecuteExpression;

            public PipelineBuilder(Expression<Func<IActivator, TResult>> createAndExecuteExpression)
            {
                this.createAndExecuteExpression = createAndExecuteExpression;
            }

            public StageBuilder<T, TResult> ThenUsing<T>()
            {
                return new StageBuilder<T, TResult>(createAndExecuteExpression);
            }

            public PipelineBuilder<TOut> ThenDo<T, TOut>(Expression<Func<T, TResult, TOut>> expression)
            {
                /// working but needs compiles!
                //Expression<Func<IActivator,TOut>> buildAndExecute = (activator) =>
                //      expression.Compile().Invoke(activator.Create<T>(), createAndExecuteExpression.Compile().Invoke(activator));
                // var InvokePreviousStepAndGetResponse = Expression.Invoke(createAndExecuteExpression, activatorParam);


                // all in expressions
                var activatorParam = Expression.Parameter(typeof(IActivator), "activator");
                Expression<Func<IActivator, T>> builder = activator => activator.Create<T>();

                var buildAndExecute =
                    Expression.Lambda<Func<IActivator, TOut>>(
                        Expression.Invoke(expression,
                            Expression.Invoke(builder, activatorParam),
                            Expression.Invoke(createAndExecuteExpression, activatorParam)
                        ), activatorParam);

                return new PipelineBuilder<TOut>(buildAndExecute);
            }

            public TResult Run(IActivator activator)
            {
                return createAndExecuteExpression.Compile()(activator);
            }
        }

        public class PipelineBuilder
        {
            public StageBuilder<T> Using<T>()
            {
                return new StageBuilder<T>();
            }

            public PipelineBuilder<TOut> Do<T, TOut>(Expression<Func<T, TOut>> expression)
            {
                // build a new expression to instantiate the type T
                Expression<Func<IActivator, T>> creator = activator => activator.Create<T>();

                // combine the builder function with the passed in expression
                var createAndExecuteExpression = ExpressionUtil.Combine(creator, expression);

                return new PipelineBuilder<TOut>(createAndExecuteExpression);
            }
        }

        public class StageBuilder<TStage> : PipelineBuilder
        {
            public PipelineBuilder<TOut> Do<TOut>(Expression<Func<TStage, TOut>> expression)
            {
                return Do<TStage, TOut>(expression);
            }
        }

        public class StageBuilder<TStep, TInput> : PipelineBuilder<TInput>
        {
            public StageBuilder(Expression<Func<IActivator, TInput>> createAndExecuteExpression) : base(
                createAndExecuteExpression)
            { }

            public PipelineBuilder<TOut> Do<TOut>(Expression<Func<TStep, TInput, TOut>> expression)
            {
                return ThenDo<TStep, TOut>(expression);
            }
        }


        public class Example
        {
            [Fact]
            public void Demo()
            {
                var pipeline = new PipelineBuilder();
                var engine = pipeline
                    .Do((A a) => a.MakeB())
                    .ThenDo((C c, B b) => c.MessWithB(b));

                var result = engine.Run(new Activator());
                Assert.Equal("Dorris", result.FullName);
            }

            [Fact]
            public void Demo2()
            {
                var pipeline = new PipelineBuilder();
                var engine = pipeline
                    .Do((A a) => a.MakeB())
                    .ThenDo((C c, B b) => c.MessWithB(b))
                    .ThenDo((C c, D dIn) => c.ReverseDName(dIn));

                var result = engine.Run(new Activator());
                Assert.Equal("sirroD", result.FullName);
            }

            [Fact]
            public void Demo3()
            {
                var pipeline = new PipelineBuilder();
                var engine = pipeline
                    .Using<A>()
                    .Do(a => a.MakeB())
                    .ThenUsing<C>()
                    .Do((c, b) => c.MessWithB(b));

                var result = engine.Run(new Activator());
                Assert.Equal("Dorris", result.FullName);
            }
        }
    }
}