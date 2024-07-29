using Microsoft.AspNetCore.SignalR;
using MyAPINetCore6.Data;
using MyAPINetCore6.Hubs;
using MyAPINetCore6.Models.Notification;
using MyAPINetCore6.Repositories;

namespace MyAPINetCore6.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHubConnectionRepository _hubConnectionRepository;
        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext, IHubConnectionRepository hubConnectionRepository)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _hubConnectionRepository = hubConnectionRepository;
        }

        public async Task<bool> CreateNotification(NotificationCreateDto notificationDto)
        {
            var noti = new Notification
            {
                UserSendId = notificationDto.UserSendId,
                UserSendName = notificationDto.UserSendName,
                UserReceiveId = notificationDto.UserReceiveId,
                Description = notificationDto.Description,
                NotificationTime = notificationDto.NotificationTime,
                NotificationType = notificationDto.NotificationType,
                IsRead = notificationDto.IsRead,
                Link = notificationDto.Link,
            };
            await _notificationRepository.CreateNotificationAsync(noti);
            // signalR thông báo trực tiếp cho tất cả các user
            //var hubConnections = await _hubConnectionRepository.GetAllHubConnectionAsync();
           // foreach (var hubConnection in hubConnections)
            //{
           // }
            await _hubContext.Clients.All.SendAsync("ReceivedNotification", noti.Description);
            return true;
        }

        public async Task<bool> CreateNotificationPersonal(NotificationCreateDto notificationDto)
        {
            var noti = new Notification
            {
                UserSendId = notificationDto.UserSendId,
                UserSendName = notificationDto.UserSendName,
                UserReceiveId = notificationDto.UserReceiveId,
                Description = notificationDto.Description,
                NotificationTime = notificationDto.NotificationTime,
                NotificationType = notificationDto.NotificationType,
                IsRead = notificationDto.IsRead,
                Link = notificationDto.Link,
            };
            await _notificationRepository.CreateNotificationAsync(noti);
            // signalR thông báo trực tiếp cho tất cả các user
            var hubConnections = await _hubConnectionRepository.GetAllHubConnectionByUserIdAsync(notificationDto.UserReceiveId);
            if(hubConnections != null)
            {
                foreach (var hubConnection in hubConnections)
                {
                    await _hubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedPersonalNotification", noti.Description, noti.UserSendName);
                }
            }
            return true;
        }

        public async Task<bool> DeleteAllNotification(string userId)
        {
            var listNoti = await _notificationRepository.GetAllNotificationByUserAsync(userId);
            if (listNoti == null)
            {
                return false;
            }
            await _notificationRepository.DeleteAllNotificationOfUserAsync(userId);
            return true;

        }

        public async Task<bool> DeleteNotification(int notificationId)
        {
            var noti = await _notificationRepository.GetNotificationByIdAsync(notificationId);
            if (noti == null)
            {
                return false;
            }
            await _notificationRepository.DeleteNotificationAsync(notificationId);
            return true;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId)
        {
            var listNoti = await _notificationRepository.GetAllNotificationByUserAsync(userId);
            if (listNoti == null)
            {
                return null;
            }
            else
            {
                return listNoti;
            }
        }



        public async Task<int> GetNumberOfNotification(string userId)
        {
            var listNoti = await _notificationRepository.GetAllNotificationByUserAsync(userId);
            if (listNoti == null)
            {
                return 0;
            }
            else
            {
                return listNoti.Count();
            }
        }

        public async Task<bool> UpdateNotification(Notification Notification)
        {
            await _notificationRepository.UpdateNotificationAsync(Notification);
            return true;
        }
    }
}
