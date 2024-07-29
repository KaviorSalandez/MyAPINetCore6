using Microsoft.EntityFrameworkCore;
using MyAPINetCore6.Data;
using MyAPINetCore6.Models.HubConnection.Dto;

namespace MyAPINetCore6.Repositories
{
    public class HubConnectionRepository : IHubConnectionRepository
    {
        private readonly BookStoreContext _context;

        public HubConnectionRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.HubConnection.Dto.HubConnection>> GetAllHubConnectionAsync()
        {
            var listConnection = await _context.HubConnections.ToListAsync();
            var listConnectioDto = listConnection.Select(x => new Models.HubConnection.Dto.HubConnection
            {
                Id = x.Id,
                ConnectionId = x.ConnectionId,
                UserId = x.UserId,
            }).ToList();
            return listConnectioDto;
        }

        public async Task<IEnumerable<Models.HubConnection.Dto.HubConnection>> GetAllHubConnectionByUserIdAsync(string UserId)
        {
            var hubConnection = await _context.HubConnections.Where(x=>x.UserId == UserId).ToListAsync();
            if (hubConnection == null)
            {
                return null;
            }
            var listConnectioDto = hubConnection.Select(x => new Models.HubConnection.Dto.HubConnection
            {
                Id = x.Id,
                ConnectionId = x.ConnectionId,
                UserId = x.UserId,
            }).ToList();
            return listConnectioDto;
        }
    }
}
