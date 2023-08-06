using Microsoft.EntityFrameworkCore;

namespace MyAPINetCore6.Data
{
    public class BookStoreContext:DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> opt):base(opt) 
        { 
        }
        #region DBSet
        public DbSet<Book>? Books { get; set; }
        #endregion
    }
}
