using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPINetCore6.Models.Notification;
using MyAPINetCore6.Services;

namespace MyAPINetCore6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationSController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationSController(INotificationService notificationService) {
            _notificationService = notificationService;
        }
        [HttpPost("CreateNotification")]
        public async Task<IActionResult> CreateNotification(NotificationCreateDto model)
        {
            var result = await _notificationService.CreateNotification(model);
            if(result == false)
            {
                return BadRequest();
            }
            return Ok("Tạo thành công");
        }
        [HttpPost("CreateNotificationPersonal")]
        public async Task<IActionResult> CreateNotificationPersonal(NotificationCreateDto model)
        {
            var result = await _notificationService.CreateNotificationPersonal(model);
            if (result == false)
            {
                return BadRequest();
            }
            return Ok("Tạo thành công");
        }
    }
}
