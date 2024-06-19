using Acef.Reasons.ApplicationCore.Entities;
using Acef.Reasons.ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Acef.Reasons.ApplicationCore.Entities.DTO;
using Acef.Reasons.ApplicationCore.Exceptions;

namespace Acef.Reasons.ApplicationCore.Services
{
    public class ReasonService : IReasonService
    {
        private readonly IAsyncRepository<Reason> _reasonRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReasonService> _logger;

        public ReasonService(IAsyncRepository<Reason> reasonRepository, IMapper mapper, ILogger<ReasonService> logger)
        {
            _reasonRepository = reasonRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Add(ReasonDTO reason)
        {
            // Check if a consultation reason already exists
            var consultationReasons = await _reasonRepository.ListAsync();

            var consultationReason = consultationReasons.FirstOrDefault(
                c => c.Name.ToLower() == reason.Name.ToLower());

            // If consultation reason exists
            if (consultationReason != null)
            {
                // If it's deleted, we can reactivate the same consultation reason
                if (consultationReason.IsDeleted)
                {
                    consultationReason.IsDeleted = false;
                    consultationReason.DeletedAt = null;
                    consultationReason.Description = reason.Description;

                    await _reasonRepository.EditAsync(consultationReason);
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
                _logger.LogInformation($"{DateTime.Now} [*] Adding consultation reason (Name = {reason.Name})...");

                await _reasonRepository.AddAsync(new Reason
                {
                    Name = reason.Name,
                    Description = reason.Description
                });
            }

        }

        public async Task Edit(int id, ReasonDTO reason)
        {
            // Check if a consultation reason exists
            var consultationReasonToEdit = await _reasonRepository.GetByIdAsync(id) ??
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Consultation reason not found"
                    }
                };

            // Check if a consultation reason already exists
            //var consultationReasons = await GetConsultationReasons();
            var consultationReasons = await _reasonRepository.ListAsync();

            if (consultationReasons.Any(c =>
                    c.Name.ToLower() == reason.Name.ToLower() &&
                    !c.IsDeleted &&
                    c.ID != consultationReasonToEdit.ID))
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        //Errors = $"{DateTime.Now} [-] Consultation reason already exists"
                        Errors = $"Consultation reason already exists"
                    }
                };
            }

            // Check if a deleted consultation reason have the same name as the one being modified
            var deletedConsultationReasonWithSameName = consultationReasons.FirstOrDefault(
                c => c.Name == reason.Name && c.IsDeleted);

            // If a deleted one exists with the same name, we can reactivate the old one...
            // ...and deactivate (delete) the new one

            if (deletedConsultationReasonWithSameName != null)
            {
                deletedConsultationReasonWithSameName.IsDeleted = false;
                deletedConsultationReasonWithSameName.DeletedAt = null;
                deletedConsultationReasonWithSameName.Description = reason.Description;

                _logger.LogInformation($"{DateTime.Now} [*] Modifying consultation reason " +
                    $"(id = {deletedConsultationReasonWithSameName.ID})...");

                await _reasonRepository.EditAsync(deletedConsultationReasonWithSameName);

                await Delete(consultationReasonToEdit.ID);
            }
            else
            {
                consultationReasonToEdit.Name = reason.Name;
                consultationReasonToEdit.Description = reason.Description;

                _logger.LogInformation($"{DateTime.Now} [*] Modifying consultation reason " +
                    $"(id = {consultationReasonToEdit.ID})...");

                await _reasonRepository.EditAsync(consultationReasonToEdit);

            }
        }

        public async Task<ReasonDTO> GetById(int id)
        {
            // Check if a consultation reason exists
            var consultationReason = await _reasonRepository.GetByIdAsync(id) ??
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

            _logger.LogInformation($"{DateTime.Now} [*] Getting consultation " +
                $"reason (id = {consultationReason.ID})...");

            return _mapper.Map<ReasonDTO>(consultationReason);
        }

        public async Task<IEnumerable<ReasonDTO>> Get()
        {
            var consultationReasons = await _reasonRepository.ListAsync();

            _logger.LogInformation($"{DateTime.Now} [*] Getting ${consultationReasons.Count()} " +
                $"consultation reason(s)...");

            // Returns only available consultation reasons
            return _mapper.Map<List<ReasonDTO>>(consultationReasons.Where(r => !r.IsDeleted));
        }

        public async Task Delete(int id)
        {
            // Check if a consultation reason exists
            var consultationReason = await _reasonRepository.GetByIdAsync(id) ?? throw new HttpException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Errors = new
                {
                    Errors = $"Consultation reason not found (id = {id})"
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

            await _reasonRepository.EditAsync(consultationReason);
        }
    }
}
