using Acef.MVC.Models.DTO;
using Acef.Raisons.ApplicationCore.Entites;
using Acef.Raisons.ApplicationCore.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acef.Raisons.ApplicationCore.Services
{
    public class RaisonService : IRaisonService
    {
        private readonly IAsyncRepository<Raison> _raisonRepository;
        private readonly IMapper _mapper;

        public RaisonService(IAsyncRepository<Raison> raisonRepository, IMapper mapper)
        {
            _raisonRepository = raisonRepository;
            _mapper = mapper;
        }

        public async Task Ajouter(RaisonDTO raison)
        {
            await _raisonRepository.AddAsync(new Raison
            {
                NomRaison = raison.NomRaison,
            });
        }

        public async Task Modifier(RaisonDTO raison)
        {
            Raison raisonAModifier = await _raisonRepository.GetByIdAsync(raison.ID);

            raisonAModifier.NomRaison = raison.NomRaison;

            await _raisonRepository.EditAsync(raisonAModifier);
        }

        public async Task<RaisonDTO> ObtenirSelonId(int id)
        {
            return _mapper.Map<RaisonDTO>(await _raisonRepository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<RaisonDTO>> ObtenirTout()
        {
            /// modification of the method to return elements to which soft delete has not been applied
            return _mapper.Map<List<RaisonDTO>>(await _raisonRepository.ListAsync(r => r.IsActive == true));
        }

        public async Task Supprimer(RaisonDTO raison)
        {
            Raison raisonASupprimer = await _raisonRepository.GetByIdAsync(raison.ID);

            /// modification of the method to apply soft delete
            raisonASupprimer.IsActive = false;
            await _raisonRepository.EditAsync(raisonASupprimer);
        }
    }
}
