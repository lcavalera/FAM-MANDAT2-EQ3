using Acef.Raisons.ApplicationCore.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
