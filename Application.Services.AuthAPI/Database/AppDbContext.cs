using Application.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.AuthAPI.Database
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1️⃣ Create Password Hasher
            var hasher = new PasswordHasher<ApplicationUser>();

            // 2️⃣ Seed Default Admin User
            var adminUser = new ApplicationUser
            {
                Id = "a88be48f-423f-4ca6-8fad-fd318e02d969",            // fixed Guid
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@thekashistay.com",
                NormalizedEmail = "ADMIN@THEKASHISTAY.COM",
                EmailConfirmed = true,
                SecurityStamp = "dec489dd-f253-4fdb-9039-8823e66fbcc7",
                ConcurrencyStamp = "d58a48e2-9f18-47c3-b7ca-46e6be2161c1",
                PasswordHash = "AQAAAAIAAYagAAAAEFi8K7Q1TB5cbXld55LTWb8L8hQvs/FUSJXD3GQK9zPvI5DOq6evDGZPV8dQU+r1Xg==",
                Name = "Administrator"  
            };

            // 4️⃣ Add to model
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Optional: Seed roles and assign role
            var adminRoleId = "a7dae5f2-e3f6-4de3-8e7f-54f568b470ad";
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUser.Id
            });
        }
    }
}
