using Mvc.StorageAccount.Demo.Models;

namespace Mvc.StorageAccount.Demo.Services
{
    public interface IQueueService
    {
        Task SendMessage(EmailMessage emailMessage);
    }
}