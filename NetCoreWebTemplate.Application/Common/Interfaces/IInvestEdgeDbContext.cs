using Microsoft.EntityFrameworkCore;
using NetCoreWebTemplate.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Application.Common.Interfaces
{
    public interface IInvestEdgeDbContext
    {
        public DbSet<Client> Clients { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
