using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Demo
{
    public static class DurableTester
    {
        [FunctionName("DurableTester")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            var input = context.GetInput<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            outputs.Add(await context.CallActivityAsync<string>(nameof(AddToQueue), input));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName(nameof(AddToQueue))]
        public static string AddToQueue([ActivityTrigger] string messageToAdd, ILogger log,
        [Queue("outqueue-durable"), StorageAccount("AzureWebJobsStorage")] ICollector<string> msg)
        {
            log.LogInformation("Message added to Queue", messageToAdd);
            msg.Add(messageToAdd);
            return $"{messageToAdd} has been added";
        }


        [FunctionName("DurableTester_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string requestBody = await req.Content.ReadAsStringAsync();
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DurableTester", null, requestBody);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}