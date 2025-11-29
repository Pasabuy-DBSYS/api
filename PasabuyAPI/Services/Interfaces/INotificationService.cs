using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;

namespace PasabuyAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationResponseDTO> CreateNotification(NotificationRequestDTO notifications);
        Task<List<NotificationResponseDTO>> GetNotifications();
        Task<NotificationResponseDTO> GetNotificationByid(long notificationId);
        Task<List<NotificationResponseDTO>> GetNotificayionsByUserId(long userId);
    }
}