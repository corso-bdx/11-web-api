using AutoMapper;
using BDX.WebApiLibrary.Model;

namespace BDX.WebApiLibrary.DTO {
    public class DtoProfiles : Profile {
        public DtoProfiles() {
            // Ruolo
            CreateMap<Ruolo, RuoloDTO>();
            CreateMap<RuoloDTO, Ruolo>();

            CreateMap<CreaRuoloDTO, Ruolo>();
            CreateMap<AggiornaRuoloDTO, Ruolo>();

            // Utente
            CreateMap<Utente, UtenteDTO>();
            CreateMap<UtenteDTO, Utente>();

            CreateMap<CreaUtenteDTO, Utente>();
            CreateMap<AggiornaUtenteDTO, Utente>();
        }
    }
}
