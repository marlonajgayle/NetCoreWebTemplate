using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreWebTemplate.Application.Common.Interfaces;
using NetCoreWebTemplate.Domain.Entities;
using NetCoreWebTemplate.Infrastructure.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Infrastructure.Persistence
{
    public class WebTemplateDbContext : IdentityDbContext<ApplicationUser>, IInvestEdgeDbContext
    {
        public WebTemplateDbContext(DbContextOptions<WebTemplateDbContext> options)
              : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
