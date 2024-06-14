using Acef.MVC.Models.DTO;
using Acef.Raisons.ApplicationCore.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acef.Raisons.ApplicationCore.Interfaces
{
    public interface IRaisonService
    {
        public Task Ajouter(RaisonDTO item);
        public Task<RaisonDTO> ObtenirSelonId(int id);
        public Task<IEnumerable<RaisonDTO>> ObtenirTout();
        public Task Modifier(RaisonDTO item);
        public Task Supprimer(RaisonDTO item);
    }
}
