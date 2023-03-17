using Blazor.AzureCosmosDb.Demo.Pages;
using Microsoft.EntityFrameworkCore;

namespace Blazor.AzureCosmosDb.Demo.Data
{
    public class EngineerServiceEfCore : IEngineerService
    {
        private readonly AzureCosmosDbContext _context;

        public EngineerServiceEfCore(AzureCosmosDbContext context)
        {
            this._context = context;
        }
        public async Task DeleteEngineer(string? id, string? partitionKey)
        {
            var engineer = await GetEngineerDetailsById(id, partitionKey);
            _context.Engineers.Remove(engineer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Engineer>> GetEngineerDetails()
        {
            var engineers = await _context.Engineers.ToListAsync();
            return engineers;
        }

        public async Task<Engineer> GetEngineerDetailsById(string? id, string? partitionKey)
        {
            var engineer = await _context.Engineers
                .WithPartitionKey(partitionKey)
                .FirstOrDefaultAsync(q => q.id.ToString() == id);

            return engineer;
        }

        public async Task UpsertEngineer(Engineer engineer)
        {
            _context.Engineers.Update(engineer);
            await _context.SaveChangesAsync();
        }
    }
}
