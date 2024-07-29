using MyAPINetCore6.Data;
using MyAPINetCore6.Models.Notification;

namespace MyAPINetCore6.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId);
        Task<int> GetNumberOfNotification(string userId);
        Task<bool> CreateNotification(NotificationCreateDto notificationDto);
        Task<bool> CreateNotificationPersonal(NotificationCreateDto notificationDto);
        Task<bool> UpdateNotification(Notification Notification);
        Task<bool> DeleteNotification(int notificationId);
        Task<bool> DeleteAllNotification(string userId);
    }
}
