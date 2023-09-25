using AutoMapper;
using Bdx.WebApiLibrary.Model;

namespace Bdx.WebApiLibrary.Dto {
    public class DtoProfiles : Profile {
        public DtoProfiles() {
            // Ruolo
            CreateMap<Ruolo, RuoloDto>();
            CreateMap<RuoloDto, Ruolo>();

            CreateMap<CreaRuoloDto, Ruolo>();
            CreateMap<AggiornaRuoloDto, Ruolo>();

            // Utente
            CreateMap<Utente, UtenteDto>();
            CreateMap<UtenteDto, Utente>();

            CreateMap<CreaUtenteDto, Utente>();
            CreateMap<AggiornaUtenteDto, Utente>();
        }
    }
}
