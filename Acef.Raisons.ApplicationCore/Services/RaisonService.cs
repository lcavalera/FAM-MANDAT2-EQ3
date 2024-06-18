using Acef.MVC.Models.DTO;
using Acef.Raisons.ApplicationCore.Entites;
using Acef.Raisons.ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ACEF.API.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Acef.Raisons.ApplicationCore.Services
{
    public class RaisonService : IRaisonService
    {
        private readonly IAsyncRepository<Raison> _raisonRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RaisonService> _logger;

        public RaisonService(IAsyncRepository<Raison> raisonRepository, IMapper mapper, ILogger<RaisonService> logger)
        {
            _raisonRepository = raisonRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Add(RaisonDTO raison)
        {
            // Check if a consultation reason already exists
            var consultationReasons = await _raisonRepository.ListAsync();

            var consultationReason = consultationReasons.FirstOrDefault(
                c => c.Name.ToLower() == raison.Name.ToLower());

            // If consultation reason exists
            if (consultationReason != null)
            {
                // If it's deleted, we can reactivate the same consultation reason
                if (consultationReason.IsDeleted)
                {
                    consultationReason.IsDeleted = false;
                    consultationReason.DeletedAt = null;
                    consultationReason.Description = raison.Description;

                    await _raisonRepository.EditAsync(consultationReason);
                }
                // If not, send an error message
                else
                {
                    throw new HttpException
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = new
                        {
                            Errors = $"Consultation reason already exists. Please choose another name."
                        }
                    };
                }
            }
            else
            {
                _logger.LogInformation($"{DateTime.Now} [*] Adding consultation reason (Name = {raison.Name})...");

                await _raisonRepository.AddAsync(new Raison
                {
                    Name = raison.Name,
                    Description = raison.Description
                });
            }

        }

        public async Task Edit(RaisonDTO raison)
        {
            // Check if a consultation reason exists
            var consultationReasonToEdit = await _raisonRepository.GetByIdAsync(raison.ID) ?? 
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Consultation reason not found"
                    }
                };

            consultationReasonToEdit.Name = raison.Name;
            consultationReasonToEdit.Description = raison.Description;

            _logger.LogInformation($"{DateTime.Now} [*] Modifying consultation reason " +
                $"(id = {consultationReasonToEdit.ID})...");

            await _raisonRepository.EditAsync(consultationReasonToEdit);
        }

        public async Task<RaisonDTO> GetById(int id)
        {
            // Check if a consultation reason exists
            var consultationReason = await _raisonRepository.GetByIdAsync(id) ??
            throw new HttpException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Errors = new
                {
                    Errors = $"{DateTime.Now} [-] Consultation reason not found (id = {id})"
                }
            };

            // Check if a consultation reason has already been deleted
            if (consultationReason.IsDeleted)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"[-] The consultation reason you are trying to access is not available. (id = {consultationReason.ID})"
                    }
                };
            }

            _logger.LogInformation($"{DateTime.Now} [*] Getting consultation reason (id = {consultationReason.ID})...");

            return _mapper.Map<RaisonDTO>(consultationReason);
        }

        public async Task<IEnumerable<RaisonDTO>> Get()
        {
            _logger.LogInformation($"{DateTime.Now} [*] Getting consultation reason(s)...");

            // Returns only available consultation reasons
            return _mapper.Map<List<RaisonDTO>>(await _raisonRepository.ListAsync(r => r.IsDeleted == false));
        }

        public async Task Delete(RaisonDTO raison)
        {
            // Check if a consultation reason exists
            var consultationReason = await _raisonRepository.GetByIdAsync(raison.ID) ?? throw new HttpException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Errors = new
                {
                    Errors = $"Consultation reason not found (id = {raison.ID})"
                }
            };

            // Check if a consultation reason has already been deleted
            if (consultationReason.IsDeleted)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"[-] Consultation reason not avaiable (id = {consultationReason.ID})"
                    }
                };
            }

            // Soft Delete
            consultationReason.IsDeleted = true;
            consultationReason.DeletedAt = DateTime.Now;

            _logger.LogInformation($"{DateTime.Now} [*] Deletion of the consultation reason (id = {consultationReason.ID})...");

            await _raisonRepository.EditAsync(consultationReason);
        }
    }
}
