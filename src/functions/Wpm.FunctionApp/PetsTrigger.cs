using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Sql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Wpm.FunctionApp;

public static class PetsTrigger
{
    [FunctionName("PetsTrigger")]
    public static void Run([SqlTrigger("[dbo].[Pets]", "WpmConnectionString")]
        IReadOnlyList<SqlChange<Pet>> changes,
        ILogger logger)
    {
        var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        var queueClient = new QueueClient(connectionString, "pets");

        foreach (var change in changes)
        {
            Pet pet = change.Item;
            logger.LogInformation($"{change.Operation} {pet.Name}");

            var message = new PetQueueItem(pet, change.Operation.ToString());
            queueClient.SendMessage(JsonConvert.SerializeObject(message));
        }

    }
}

public record Pet(int Id, string Name, int? Age, decimal? Weight);
public record PetQueueItem(Pet Pet, string Operation);


