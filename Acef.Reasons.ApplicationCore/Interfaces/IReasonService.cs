using Acef.Reasons.ApplicationCore.Entities.DTO;

namespace Acef.Reasons.ApplicationCore.Interfaces
{
    public interface IReasonService
    {
        public Task Add(ReasonDTO item);
        public Task<ReasonDTO> GetById(int id);
        public Task<IEnumerable<ReasonDTO>> Get();
        public Task Edit(int id, ReasonDTO item);
        public Task Delete(int id);
    }
}
