using BDX.WebApiLibrary.DTO;

namespace BDX.WebApiLibrary.Data.Repository;
public interface IWebApiDtoRepository {

    #region Ruolo

    RuoloDTO Create(CreaRuoloDTO creaRuolo);

    List<RuoloDTO> Read(RicercaRuoloDTO ricercaRuolo);

    RuoloDTO Update(AggiornaRuoloDTO aggiornaRuolo);

    void Delete(EliminaRuoloDTO eliminaRuolo);

    #endregion

    #region Utente
    UtenteDTO Create(CreaUtenteDTO creaUtente);

    List<UtenteDTO> Read(RicercaUtenteDTO ricercaUtente);

    UtenteDTO Update(AggiornaUtenteDTO aggiornaUtente);

    void Delete(EliminaUtenteDTO eliminaUtente);

    void AggiornaPassword(AggiornaPasswordDTO aggiornaPassword);

    bool VerificaPassword(VerificaPasswordDTO verificaPassword);

    #endregion

}
