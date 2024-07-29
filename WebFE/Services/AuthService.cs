using WebFE.Models;

namespace WebFE.Services
{
    public class AuthService
    {
        private readonly HttpClient _client;
        public AuthService(HttpClient client)
        {
            _client = client;
        }
        public async Task<bool> RegisterAsync(SignUpModel model)
        {
            var response = await _client.PostAsJsonAsync("https://localhost:7052/api/Accounts/SignUp", model);
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> LoginAsync(SignInModel model)
        {
            var response = await _client.PostAsJsonAsync("https://localhost:7052/api/Accounts/SignIn", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
public class AuthResult
{
    public string Token { get; set; }
}
