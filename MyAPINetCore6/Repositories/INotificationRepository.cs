using MyAPINetCore6.Data;

namespace MyAPINetCore6.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<Notification> UpdateNotificationAsync(Notification notification);
        Task<bool> DeleteNotificationAsync(int id);     
        Task<bool> DeleteAllNotificationOfUserAsync(string  userId);     
        Task<Notification> GetNotificationByIdAsync(int id);     
        Task<IEnumerable<Notification>> GetAllNotificationAsync();     
        Task<IEnumerable<Notification>> GetAllNotificationByUserAsync(string userId);     
    }
}
