using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CleanArchitecture.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<UserVacancies> UserVacancies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<UserVacancies>()
                .HasKey(uv => new { uv.VacancyId, uv.UserId });
        }
    }
}
