using Acef.MVC.Models.DTO;

namespace Acef.MVC.Interfaces
{
    public interface IReasonService
    {
        public Task Add(ReasonDTO item);
        public Task<ReasonDTO> GetById(int id);
        public Task<IEnumerable<ReasonDTO>> Get();
        public Task Edit(ReasonDTO item);
        public Task Delete(ReasonDTO item);
    }
}
