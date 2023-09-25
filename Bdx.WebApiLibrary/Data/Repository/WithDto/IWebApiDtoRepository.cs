using Bdx.WebApiLibrary.Dto;

namespace Bdx.WebApiLibrary.Data.Repository;

public interface IWebApiDtoRepository {
    #region Ruolo

    RuoloDto Create(CreaRuoloDto creaRuolo);

    List<RuoloDto> Read(RicercaRuoloDto ricercaRuolo);

    RuoloDto Update(AggiornaRuoloDto aggiornaRuolo);

    void Delete(EliminaRuoloDto eliminaRuolo);

    #endregion

    #region Utente

    UtenteDto Create(CreaUtenteDto creaUtente);

    List<UtenteDto> Read(RicercaUtenteDto ricercaUtente);

    UtenteDto Update(AggiornaUtenteDto aggiornaUtente);

    void Delete(EliminaUtenteDto eliminaUtente);

    void AggiornaPassword(AggiornaPasswordDto aggiornaPassword);

    bool VerificaPassword(VerificaPasswordDto verificaPassword);

    #endregion
}
