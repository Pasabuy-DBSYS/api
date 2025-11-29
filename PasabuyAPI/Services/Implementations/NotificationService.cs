using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class NotificationService(INotificationRepository notificationRepository) : INotificationService
    {
        public async Task<NotificationResponseDTO> CreateNotification(NotificationRequestDTO notifications)
        {
            var response = await notificationRepository.CreateNotification(notifications.Adapt<Notifications>());
            return response.Adapt<NotificationResponseDTO>();
        }

        public async Task<NotificationResponseDTO> GetNotificationByid(long notificationId)
        {
            var response = await notificationRepository.GetNotificationByid(notificationId);
            return response.Adapt<NotificationResponseDTO>();
        }

        public async Task<List<NotificationResponseDTO>> GetNotifications()
        {
            var response = await notificationRepository.GetNotifications();
            return response.Adapt<List<NotificationResponseDTO>>();
        }

        public async Task<List<NotificationResponseDTO>> GetNotificayionsByUserId(long userId)
        {
            var response = await notificationRepository.GetNotificayionsByUserId(userId);
            return response.Adapt<List<NotificationResponseDTO>>();
        }

    }
}