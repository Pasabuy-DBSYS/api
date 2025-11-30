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
        Task<NotificationResponseDTO> DeleteNotificationById(long notificationId);
        Task<NotificationResponseDTO> ReadNotificationById(long notificationId);
        Task<List<NotificationResponseDTO>> ReadAllNotificationByUserId(long userId);
    }
}