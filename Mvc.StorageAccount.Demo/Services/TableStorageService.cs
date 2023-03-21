using Azure;
using Azure.Data.Tables;
using Mvc.StorageAccount.Demo.Data;
using System.Collections.Concurrent;

namespace Mvc.StorageAccount.Demo.Services
{
    public class TableStorageService : ITableStorageService
    {
        private const string tableName = "Attendees";
        private readonly IConfiguration _configuration;
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableClient _tableClient;

        public TableStorageService(IConfiguration configuration, TableServiceClient tableServiceClient, TableClient tableClient)
        {
            this._configuration = configuration;
            this._tableServiceClient = tableServiceClient;
            this._tableClient = tableClient;
        }

        public async Task<AttendeeEntity> GetAttendee(string industry, string id)
        {
            //var tableClient = _tableServiceClient.GetTableClient(tableName); 
            return await _tableClient.GetEntityAsync<AttendeeEntity>(industry, id);
        }

        public async Task<List<AttendeeEntity>> GetAttendees()
        {
            //var tableClient = _tableServiceClient.GetTableClient(tableName);
            Pageable<AttendeeEntity> attendeeEntities = _tableClient.Query<AttendeeEntity>();
            return attendeeEntities.ToList();
        }

        public async Task UpsertAttendee(AttendeeEntity attendeeEntity)
        {
            //var tableClient = _tableServiceClient.GetTableClient(tableName);
            await _tableClient.UpsertEntityAsync(attendeeEntity);
        }

        public async Task DeleteAttendee(string industry, string id)
        {
            //var tableClient = _tableServiceClient.GetTableClient(tableName);
            await _tableClient.DeleteEntityAsync(industry, id);
        }

        private async Task<TableClient> GetTableClient()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();

            return tableClient;
        }
    }
}
