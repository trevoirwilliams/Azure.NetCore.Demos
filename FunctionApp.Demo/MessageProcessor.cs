using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Demo
{
    public class MessageProcessor
    {
        [FunctionName("MessageProcessor")]
        public void Run([QueueTrigger("message-queue", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            // Send an email
            // validate
            // alert someone
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
