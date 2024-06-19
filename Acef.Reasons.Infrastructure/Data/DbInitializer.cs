using Acef.Reasons.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acef.Reasons.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ReasonsDbContext context)
        {
            if (context.Reasons.Any())
            {
                return; // DB has been seeded
            }

            // Consultation Reasons
            var consultationReasons = new Reason[]
            {
                new() { Name = "Demande de prêt", Description = "Une raison de consultation pour une demande de prêt" },
                new() { Name = "Endettement" },
                new() { Name = "Hydro-Québec", Description = "Une raison de consultation concernant Hydro-Québec" },

                new() { Name = "Méthode Budgétaire" },
                new() { Name = "Suivi Prêt" },
                new() { Name = "Autre / Projet", Description = "Une description pour un autre projet" },
            };

            context.Reasons.AddRange(consultationReasons);

            context.SaveChanges();
        }
    }
}
