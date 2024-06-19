using Acef.Reasons.ApplicationCore.Entities.DTO;
using AutoMapper;

namespace Acef.Reasons.ApplicationCore.Entities.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Reason, ReasonDTO>();
            CreateMap<ReasonDTO, Reason>();
        }

    }
}
