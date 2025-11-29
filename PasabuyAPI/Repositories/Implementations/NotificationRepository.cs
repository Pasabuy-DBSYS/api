using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class NotificationRepository(PasabuyDbContext _context) : INotificationRepository
    {
        public async Task<Notifications> CreateNotification(Notifications notifications)
        {
            _context.Notifications.Add(notifications);
            await _context.SaveChangesAsync();

            return notifications;
            
        }

        public async Task<Notifications> GetNotificationByid(long notificationId)
        {
            return await _context.Notifications.FindAsync(notificationId) ?? 
                    throw new NotFoundException($"Notification ID: {notificationId} not found");
        }

        public async Task<List<Notifications>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<List<Notifications>> GetNotificayionsByUserId(long userId)
        {
            return await _context.Notifications.Where(n => n.UserIdFk == userId).ToListAsync();
        }
    }
}