using System;
using System.Threading.Tasks;
using Xunit;

namespace NettleIO.Core.Tests
{
    public class UsingTheFluentApiExamples
    {
        private class IntroduceYourselfStage
        {
            public async Task<IStageValueResult<string>> SayHi()
            {
                return await Result.SuccessWithValueAsync("Hi Joe. Nice to see you.");
            }
        }

        private class GentrifierStage
        {
            public async Task<IStageValueResult<string>> FormaliseGreeting(string greeting)
            {
                var formailsedGreeting = greeting.Replace("Hi", "Good day").Replace("Joe", "Mr Bloggs");
                return await Result.SuccessWithValueAsync(formailsedGreeting);
            }
        }

        private class RecieveGreetingStage
        {
            public static string LastGreetingRecieved;

            public async Task<IStageResult> Listen(string greeting)
            {
                LastGreetingRecieved = greeting;
                Console.WriteLine($"Recieved: {greeting}");
                return await Result.SuccessAsync();
            }
        }

        [Fact]
        public async Task PipelineSaysHello()
        {
            var builder =
                PipelineBuilder.StartWithSource<IntroduceYourselfStage>()
                    .AndDo(stage => stage.SayHi())
                    .ThenAddStage<GentrifierStage>()
                    .AndDo((poshMaker, greeting) => poshMaker.FormaliseGreeting(greeting))
                    .ThenAddStage<RecieveGreetingStage>()
                    .AndDo((stage, message) => stage.Listen(message));

            var pipeline = builder.Build();

            var result = await pipeline.Execute();
            Assert.True(result.Succeeded);
            Assert.Equal("Good day Mr Bloggs. Nice to see you.", RecieveGreetingStage.LastGreetingRecieved);
        }
    }
}