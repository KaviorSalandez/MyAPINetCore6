using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using WebFE.Dtos.NotificationDto;
using WebFE.Models;
using WebFE.Services;

namespace WebFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NotiService notiService;
        public HomeController(NotiService notiService, IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
            this.notiService = notiService;
        }

        public IActionResult Index()
        {
       
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendToAllForm()
        {
            var user = _httpContextAccessor.HttpContext.User;


            var notiDto = new NotificationCreateDto()
            {
                UserSendId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
                UserSendName = user.Identity.Name,
                UserReceiveId = null,
                Description = "Anh yêu tất cả các em nào được gửi thông báo tới",
                NotificationTime = DateTime.Now,
                NotificationType = 1,
                IsRead = false,
                Link = null
            };
            await notiService.CreateNotifi(notiDto);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SendToUserForm()
        {
            var user = _httpContextAccessor.HttpContext.User;


            var notiDto = new NotificationCreateDto()
            {
                UserSendId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
                UserSendName = user.Identity.Name,
                UserReceiveId = "534f5d5c-0860-4291-8af5-8cfddac845d9",
                Description = "Gửi riêng đến nàng, người yêu của anh",
                NotificationTime = DateTime.Now,
                NotificationType = 2,
                IsRead = false,
                Link = null
            };
            await notiService.CreateNotifiPersonal(notiDto);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
