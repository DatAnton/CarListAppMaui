using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarListApi
{
    public class CarListDbContext : IdentityDbContext
    {
        public CarListDbContext(DbContextOptions<CarListDbContext> options) : base(options)
        {
            
        }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().HasData(new List<Car>
            {
                new Car
                {
                    Id = 1, Make = "Honda", Model = "Fit", Vin = "123"
                },
                new Car
                {
                    Id = 2, Make = "Toyota", Model = "Prado", Vin = "123"
                },
                new Car
                {
                    Id = 3, Make = "Honda", Model = "Civic", Vin = "123"
                },
                new Car
                {
                    Id = 4, Make = "Audi", Model = "A5", Vin = "123"
                },
                new Car
                {
                    Id = 5, Make = "BMW", Model = "M3", Vin = "123"
                },
                new Car
                {
                    Id = 6, Make = "Nissan", Model = "Note", Vin = "123"
                },
                new Car
                {
                    Id = 7, Make = "Ferrari", Model = "Spider", Vin = "123"
                },
            });

            modelBuilder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "546bc2c7-db8d-4634-9bb9-a315bf9a2898",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "3306930e-624c-4790-984a-b95632753e8b",
                    Name = "User",
                    NormalizedName = "USER"
                }
            });

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "408aa945-3d84-4421-8342-7269ec64d949",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    UserName = "admin@localhost.com",
                    PasswordHash = hasher.HashPassword(null, "P@ssw0rd"),
                    EmailConfirmed = true
                },
                new IdentityUser
                {
                    Id = "3f4631bd-f907-4409-b416-ba356312e659",
                    Email = "user@localhost.com",
                    NormalizedEmail = "USER@LOCALHOST.COM",
                    NormalizedUserName = "USER@LOCALHOST.COM",
                    UserName = "user@localhost.com",
                    PasswordHash = hasher.HashPassword(null, "P@ssw0rd"),
                    EmailConfirmed = true
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "546bc2c7-db8d-4634-9bb9-a315bf9a2898",
                    UserId = "408aa945-3d84-4421-8342-7269ec64d949"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "3306930e-624c-4790-984a-b95632753e8b",
                    UserId = "3f4631bd-f907-4409-b416-ba356312e659"
                }
            );

        }
    }
}