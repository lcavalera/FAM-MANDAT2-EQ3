using Acef.MVC.Models.DTO;

namespace Acef.MVC.Interfaces
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
