using LomboReaderAPI.Data.Entety;
using Microsoft.EntityFrameworkCore;

namespace LomboReaderAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookArticle> BookArticles { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<SubGenre> SubGenres { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            #region
            //if (!Database.EnsureCreated())
            //{
            //    Genre g1 = new() { GenreName = "Фантастика" };
            //    Genre g2 = new() { GenreName = "Фэнтези" };
            //    Genre g3 = new() { GenreName = "Детективы" };
            //    Genre g4 = new() { GenreName = "Проза" };
            //    Genre g5 = new() { GenreName = "Любовные романы" };
            //    Genre g6 = new() { GenreName = "Приключения" };
            //    Genre g7 = new() { GenreName = "Детское" };
            //    Genre g8 = new() { GenreName = "Дом и семья" };
            //    Genre g9 = new() { GenreName = "Поэзия, Драматургия" };
            //    Genre g10 = new() { GenreName = "Старинная литература" };
            //    Genre g11 = new() { GenreName = "Наука, Образование" };
            //    Genre g12 = new() { GenreName = "Компьютеры и Интернет" };
            //    Genre g13 = new() { GenreName = "Справочная литература" };
            //    Genre g14 = new() { GenreName = "Документальное" };
            //    Genre g15 = new() { GenreName = "Религия и духовность" };
            //    Genre g16 = new() { GenreName = "Юмор" };

            //    Genres.Add(g1);
            //    Genres.Add(g2);
            //    Genres.Add(g3);
            //    Genres.Add(g4);
            //    Genres.Add(g5);
            //    Genres.Add(g6);
            //    Genres.Add(g7);
            //    Genres.Add(g8);
            //    Genres.Add(g9);
            //    Genres.Add(g10);
            //    Genres.Add(g11);
            //    Genres.Add(g12);
            //    Genres.Add(g13);
            //    Genres.Add(g14);
            //    Genres.Add(g15);
            //    Genres.Add(g16);

            //    Role role = new() { RoleName = "Admin" };
            //    Role role1 = new() { RoleName = "Customer" };

            //    Roles.Add(role);
            //    Roles.Add(role1);

            //    SaveChanges();
            //};
            #endregion
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.HasDefaultSchema("LimboReaderDB");

          modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.Parse("7180e255-c840-4180-9384-12d3f063e444"),
                    RoleName = "Admin"
                },
                new Role
                {
                    Id = Guid.Parse("7fa1237a-1756-4ab6-a789-2d3a5330ae78"),
                    RoleName = "UserController"
                }
                );

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
