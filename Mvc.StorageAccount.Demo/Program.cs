using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using Mvc.StorageAccount.Demo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var storageConnectionString = builder.Configuration["AzureStorage:ConnectionString"];
builder.Services.AddAzureClients(builder =>
{
    builder.AddBlobServiceClient(storageConnectionString);
    builder.AddQueueServiceClient(storageConnectionString)
    .ConfigureOptions(c =>
    {
        c.MessageEncoding = QueueMessageEncoding.Base64;
    });
    builder.AddTableServiceClient(storageConnectionString);
});

builder.Services.AddAzureClients(b => {
    b.AddClient<QueueClient, QueueClientOptions>((_, _, _) =>
    {
        return new QueueClient(storageConnectionString,
                builder.Configuration["AzureStorage:QueueName"],
                new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                });
    });

    b.AddClient<TableClient, TableClientOptions>((_, _, _) =>
    {
        return new TableClient(storageConnectionString,
                builder.Configuration["AzureStorage:TableStorage"]);
    });
});

builder.Services.AddScoped<ITableStorageService, TableStorageService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
