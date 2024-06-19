using Acef.Reasons.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acef.Reasons.Infrastructure.Data
{
    public class ReasonsDbContext : DbContext
    {
        public ReasonsDbContext(DbContextOptions<ReasonsDbContext> options) : base(options)
        {
        }

        public DbSet<Reason> Reasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reason>().ToTable("REASON");
        }
    }
}
