using Acef.MVC.Models.DTO;
using AutoMapper;

namespace Acef.Raisons.ApplicationCore.Entites.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Raison, RaisonDTO>();
        }

    }
}
