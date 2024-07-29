using Microsoft.EntityFrameworkCore;
using MyAPINetCore6.Data;

namespace MyAPINetCore6.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly BookStoreContext _context;
        public NotificationRepository(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteAllNotificationOfUserAsync(string userId)
        {
            var listNoti = await GetAllNotificationByUserAsync(userId);
            if(listNoti != null) {
                _context.Notifications.RemoveRange(listNoti);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var noti = await GetNotificationByIdAsync(id);
            if (noti == null)
            {
                return false;
            }
            _context.Notifications.Remove(noti);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationAsync()
        {
            var list = await _context.Notifications.ToListAsync();
            return list;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationByUserAsync(string userId)
        {
            var list = _context.Notifications.Where(x=>x.UserReceiveId == userId).ToList();
            return list;
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            var noti = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (noti == null)
            {
                return null;
            }
            else
            {
                return noti;
            }
        }

        public async Task<Notification> UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
    }
}
