using Microsoft.AspNetCore.Identity;
using MyAPINetCore6.Models;

namespace MyAPINetCore6.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
        
    }
}
