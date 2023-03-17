using Blazor.AzureCosmosDb.Demo.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("CosmosDbConnection");
var dbName = builder.Configuration["CosmosDbDetails:CosmosDbName"];
builder.Services.AddDbContext<AzureCosmosDbContext>(options => options.UseCosmos(
    accountKey: builder.Configuration["CosmosDbDetails:CosmosDbKey"], 
    accountEndpoint: builder.Configuration["CosmosDbDetails:CosmosDbEndpoint"],
    databaseName: builder.Configuration["CosmosDbDetails:CosmosDbName"]));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddScoped<IEngineerService, EngineerService>();
builder.Services.AddScoped<IEngineerService, EngineerServiceEfCore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
