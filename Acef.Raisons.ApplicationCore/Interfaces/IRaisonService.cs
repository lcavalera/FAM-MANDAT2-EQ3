using Acef.MVC.Models.DTO;

namespace Acef.Raisons.ApplicationCore.Interfaces
{
    public interface IRaisonService
    {
        public Task Add(RaisonDTO item);
        public Task<RaisonDTO> GetById(int id);
        public Task<IEnumerable<RaisonDTO>> Get();
        public Task Edit(RaisonDTO item);
        public Task Delete(RaisonDTO item);
    }
}
