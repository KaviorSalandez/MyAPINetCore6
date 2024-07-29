using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebFE.Models;
using WebFE.Services;

namespace WebFE.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        // when we are working with the cookies, we need to inject the HTTP context accessor.
        private readonly IHttpContextAccessor _contextAccessor;
        public AccountController(AuthService authService, IHttpContextAccessor contextAccessor)
        {
            _authService = authService;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(model);
                if (result==true)
                {
                    return RedirectToAction("Login");
                }
                return View(model);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _authService.LoginAsync(model);
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);

                    //And from that token, now we want to extract all the claims that we added.
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                                            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
                    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                                            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
                    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                                            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
                    identity.AddClaim(new Claim(ClaimTypes.Name,
                                  jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    SetToken(token);
                    var test = GetToken();
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await ClearToken();
            await HttpContext.SignOutAsync();
            var test = GetToken();
            return RedirectToAction("Login", "Account");
        }
        public async Task ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete("JWTToken");
        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = (_contextAccessor.HttpContext?.Request.Cookies.TryGetValue("JWTToken", out token));
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append("JWTToken", token);
        }
    }
}
