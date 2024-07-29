using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyAPINetCore6.Data
{
    public class BookStoreContext:IdentityDbContext<ApplicationUser>
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> opt):base(opt) 
        { 
        }
        #region DBSet
        public DbSet<Book>? Books { get; set; }
        public DbSet<HubConnection>? HubConnections { get; set; }
        public DbSet<Notification>? Notifications { get; set; }
        #endregion
    }
}
