using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "";

const string queueName = "";
const string topicName = "";
const string sub1Name = "";
ServiceBusClient client;
ServiceBusProcessor processor = default!;

async Task MessageHandler(ProcessMessageEventArgs processMessageEventArgs)
{
    string body = processMessageEventArgs.Message.Body.ToString();
    Console.WriteLine($"{body} - Subscription: {sub1Name}");
    //Console.WriteLine($"{body}");
    await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
}

Task ErrorHandler(ProcessErrorEventArgs processMessageEventArgs)
{
    Console.WriteLine(processMessageEventArgs.Exception.ToString());
    return Task.CompletedTask;
}

client = new ServiceBusClient(serviceBusConnectionString);
//processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
processor = client.CreateProcessor(topicName, sub1Name, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Press any key to end the processing");
    Console.ReadKey();

    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}
