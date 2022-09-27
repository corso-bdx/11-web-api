using BDX.WebApiLibrary.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BDX.WebApiLibrary.Data.Repository;
public class WebApiModelRepository : IWebApiModelRepository, IDisposable {
    private readonly WebApiDbContext _context;

    public WebApiModelRepository(IDbContextFactory<WebApiDbContext> factory) {
        _context = factory.CreateDbContext();
    }

    public bool SaveChanges() => _context.SaveChanges() >= 0;
    public void Dispose() => _context.Dispose();

    #region Ruolo

    public List<Ruolo> ElencoRuoli() {
        return _context.ListaRuoli.ToList();
    }

    public Ruolo? CercaRuolo(string nomeRuolo) {
        return _context.ListaRuoli.SingleOrDefault(p => p.Nome == nomeRuolo);
    }

    public void InserisciRuolo(Ruolo ruoloDaCreare) {
        Ruolo? ruoloPresenteSuDB = CercaRuolo(ruoloDaCreare.Nome);

        if (ruoloPresenteSuDB is not null)
            throw new Exception($"Ruolo {ruoloDaCreare.Nome} già censito!");

        _context.ListaRuoli.Add(ruoloDaCreare);
        _context.SaveChanges();
    }

    public void ModificaRuolo(Ruolo ruoloModificato) {
        Ruolo? ruoloPresenteSuDB = CercaRuolo(ruoloModificato.Nome);

        if (ruoloPresenteSuDB is null)
            throw new Exception($"Ruolo {ruoloModificato.Nome} non censito. Impossibile modificare!");

        ruoloPresenteSuDB.Descrizione = ruoloModificato.Descrizione;
        ruoloPresenteSuDB.Categoria = ruoloModificato.Categoria;

        _context.SaveChanges();
    }

    public void EliminaRuolo(string nomeRuolo) {
        _context.ListaRuoli.Remove(CercaRuolo(nomeRuolo) ?? throw new Exception($"Ruolo {nomeRuolo} non censito. Impossibile eliminare!"));

        _context.SaveChanges();
    }

    #endregion

    #region Utente

    public List<Utente> ElencoUtenti() {
        return _context.ListaUtenti.ToList();
    }

    public Utente? CercaUtente(string nomeUtente) {
        return _context.ListaUtenti.SingleOrDefault(p => p.NomeUtente == nomeUtente);
    }

    public void InserisciUtente(Utente utenteDaCreare) {
        Utente? utentePresenteSuDB = CercaUtente(utenteDaCreare.NomeUtente);

        if (utentePresenteSuDB is not null)
            throw new Exception($"Utente {utenteDaCreare.Nome} già censito!");

        utenteDaCreare!.Password = GetHash(utenteDaCreare.Password, out string salt);
        utenteDaCreare.Salt = salt;
        utenteDaCreare.DataUltimoCambioPassword = DateTime.Now;
        
        _context.ListaUtenti.Add(utenteDaCreare);
        _context.SaveChanges();
    }

    public void ModificaUtente(Utente utenteModificato) {
        Utente? utentePresenteSuDB = CercaUtente(utenteModificato.NomeUtente);

        if (utentePresenteSuDB is null)
            throw new Exception($"Utente {utenteModificato.Nome} non censito. Impossibile modificare!");

        utentePresenteSuDB = utenteModificato;
        utentePresenteSuDB.Password = GetHash(utenteModificato.Password, out string salt);
        utentePresenteSuDB.Salt = salt;
        utentePresenteSuDB.DataUltimoCambioPassword = DateTime.Now;

        //_context.ListaUtenti.Update(utenteModificato);
        _context.SaveChanges();
    }

    public void EliminaUtente(string nomeUtente) {
        _context.ListaUtenti.Remove(CercaUtente(nomeUtente) ?? throw new Exception($"Utente {nomeUtente} non censito. Impossibile eliminare!"));

        _context.SaveChanges();
    }

    public void AggiornaPassword(string nomeUtente, string password) {
        Utente? utentePresenteSuDB = CercaUtente(nomeUtente);

        if (utentePresenteSuDB is null)
            throw new Exception($"Utente {nomeUtente} non censito. Impossibile modificare la password!");

        utentePresenteSuDB.Password = GetHash(password, out string salt);
        utentePresenteSuDB.Salt = salt;
        utentePresenteSuDB.DataUltimoCambioPassword = DateTime.Now;

        _context.SaveChanges();
    }

    public bool VerificaPassword(string nomeUtente, string password) {
        Utente? utentePresenteSuDB = CercaUtente(nomeUtente);

        if (utentePresenteSuDB is null)
            throw new Exception($"Utente {nomeUtente} non censito. Impossibile verificare la password!");

        return utentePresenteSuDB.Password == GetHash(password, utentePresenteSuDB.Salt);
    }

    private string GetHash(string password, out string salt) {
        salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));

        return GetHash(password, salt);
    }

    private string GetHash(string password, string salt) {
        return Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8));
    }

    #endregion
}