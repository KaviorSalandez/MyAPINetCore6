using WebFE.Dtos.NotificationDto;

namespace WebFE.Services
{
    public class NotiService
    {
        private readonly HttpClient _client;
        public NotiService(HttpClient client)
        {
            _client = client;
        }
        public async Task<bool> CreateNotifi(NotificationCreateDto model)
        {
            var response = await _client.PostAsJsonAsync("https://localhost:7052/api/Notifications/CreateNotification", model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> CreateNotifiPersonal(NotificationCreateDto model)
        {
            var response = await _client.PostAsJsonAsync("https://localhost:7052/api/Notifications/CreateNotificationPersonal", model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
