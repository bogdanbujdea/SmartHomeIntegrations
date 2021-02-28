using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartHomeIntegrations.Core.Infrastructure;

namespace SmartHomeIntegrations.Core.Entities
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IConfiguration configuration, IOptions<ServerSettings> serverSettings)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(_configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(_configuration["ConnectionStrings:DefaultConnection"]));
        }

        public DbSet<State> States { get; set; }
    }
}