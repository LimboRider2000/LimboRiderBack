using LimboReaderAPI.Data.Entety;
using Microsoft.EntityFrameworkCore;

namespace LimboReaderAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookArticle> BookArticles { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<SubGenre> SubGenres { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.HasDefaultSchema("LimboReaderDB");
                  
           modelBuilder.Entity<User>().HasData(
                    new User()
                    {
                        Id = Guid.Parse("6764db38-5306-4e85-a15d-7392e8422b8a"),
                        Email = "admin@ukr.net",
                        Login = "Admin",
                        Name = "Root Administrator",
                        Avatar = null,
                        PasswordHash = "CDE2E51D593001C6392593A3332BCEE452B61273",
                        RegisterDt = DateTime.Now,
                        UserRole = "Admin"
                    }
                ) ;

        }
    }
}
