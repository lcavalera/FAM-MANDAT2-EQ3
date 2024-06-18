using Acef.Raisons.ApplicationCore.Entites;
using Microsoft.EntityFrameworkCore;

namespace Acef.Raisons.Infrastructure.Data
{
    public class RaisonContext: DbContext
    {
        public RaisonContext(DbContextOptions<RaisonContext> options) : base(options)
        {

        }

        public DbSet<Raison> Raison { get; set; }
    }
}
