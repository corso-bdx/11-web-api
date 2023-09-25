using Bdx.WebApiLibrary.Model;

namespace Bdx.WebApiLibrary.Data.Repository;

public interface IWebApiModelRepository {
    #region Ruolo

    List<Ruolo> ElencoRuoli();
    Ruolo? CercaRuolo(string nomeRuolo);
    void InserisciRuolo(Ruolo ruoloDaCreare);
    void ModificaRuolo(Ruolo ruoloModificato);
    void EliminaRuolo(string nomeRuolo);

    #endregion

    #region Utente

    List<Utente> ElencoUtenti();
    Utente? CercaUtente(string nomeUtente);
    void InserisciUtente(Utente utenteDaCreare);
    void ModificaUtente(Utente utenteModificato);
    void EliminaUtente(string nomeUtente);
    void AggiornaPassword(string nomeUtente, string password);
    bool VerificaPassword(string nomeUtente, string password);

    #endregion
}
