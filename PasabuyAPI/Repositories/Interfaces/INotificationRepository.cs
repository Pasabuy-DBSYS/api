using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notifications> CreateNotification(Notifications notifications);
        Task<List<Notifications>> GetNotifications();
        Task<Notifications> GetNotificationByid(long notificationId);
        Task<List<Notifications>> GetNotificayionsByUserId(long userId);
    }
}