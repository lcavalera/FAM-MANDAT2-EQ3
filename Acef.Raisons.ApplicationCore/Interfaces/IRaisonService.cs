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
        public Task Add(RaisonDTO item);
        public Task<RaisonDTO> GetById(int id);
        public Task<IEnumerable<RaisonDTO>> Get();
        public Task Edit(RaisonDTO item);
        public Task Delete(RaisonDTO item);
    }
}
