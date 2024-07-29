using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MyAPINetCore6.Data;
using MyAPINetCore6.Helpers;
using MyAPINetCore6.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyAPINetCore6.Repositories
{

    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AccountRepository(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            //inject 
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager; 

        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if(user == null || !passwordValid)
            {
                return string.Empty;
            }
            var authClaims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email,model.Email),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name,user.FirstName + " "+ user.LastName),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }



            var authenKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience : configuration["JWT:ValidAudience"],
                    expires:DateTime.Now.AddMinutes(20),
                    claims:authClaims,
                    signingCredentials: new SigningCredentials(authenKey,SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };
            var result =  await userManager.CreateAsync(user, model.Password);// tự động lưu vào aspnetUser

            // after create user, add role default for this user (Customer)

            if(result.Succeeded)
            {
                // check role Customer exist ??
                if(!await roleManager.RoleExistsAsync(AppRole.Customer))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                }
                // regis role to user
                await userManager.AddToRoleAsync(user, AppRole.Customer);
            }


            return result;
        }
    }
}
