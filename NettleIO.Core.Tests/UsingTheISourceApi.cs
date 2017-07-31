﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NettleIO.Core.Tests
{
    public class SampleUsage
    {
        [Fact]
        public async Task DoStuff()
        {
            IPipeline pipeline = new Pipeline(new Activator());
            pipeline
                .AddSource<MySource,MyEntity>()
                .AddStage<CapitaliseNameStage, MyEntity>()
                .AddStage<MapToAnotherStage,MyEntity,AnotherEntity>()
                .RegisterDestination<MyDestination, AnotherEntity>();

            await pipeline.Execute();
        }

        class MyEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class AnotherEntity
        {
            public int Id { get; set; }
            public string FullName { get; set; }
        }

        class MySource : Source<MyEntity>
        {
            public override Task<IValueResult<MyEntity>> RecieveAsync()
            {
                Console.WriteLine("Created some data...");
                return SuccessAsync(new MyEntity {Id = 1, Name = "Blunderbuss"});
            }
        }

        class CapitaliseNameStage : Stage<MyEntity>
        {
            public override async Task<IValueResult<MyEntity>> Execute(MyEntity input)
            {
                Console.WriteLine("Capitalising some thing");
                await Task.Delay(100);


                input.Name = input.Name.ToUpper();
                return Success(input);
            }
        }

        class MapToAnotherStage : Stage<MyEntity,AnotherEntity>
        {
            public override async Task<IValueResult<AnotherEntity>> Execute(MyEntity input)
            {
                Console.WriteLine("Mapping to another!");
                await Task.Delay(100);


                input.Name = input.Name.ToUpper();
                return Success(new AnotherEntity(){Id=input.Id, FullName = input.Name});
            }
        }

        class MyDestination : Destination<AnotherEntity>
        {
            private readonly List<AnotherEntity> results = new List<AnotherEntity>();

            public override async Task<IActionResult> SendAsync(AnotherEntity item)
            {
                results.Add(item);
                Console.WriteLine($"Recieved item with ID {item.Id} and name {item.FullName}");
                return await Result.SuccessAsync("woop");
            }
        }
    }
}