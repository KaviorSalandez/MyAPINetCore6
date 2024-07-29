using MyAPINetCore6.Models;
using MyAPINetCore6.Models.HubConnection.Dto;

namespace MyAPINetCore6.Repositories
{
    public interface IHubConnectionRepository
    {
        Task<IEnumerable<HubConnection>> GetAllHubConnectionAsync();
        Task<IEnumerable<HubConnection>> GetAllHubConnectionByUserIdAsync(string UserId);
    }
}
