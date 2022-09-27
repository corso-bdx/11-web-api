using AutoMapper;
using BDX.WebApiLibrary.DTO;
using BDX.WebApiLibrary.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BDX.WebApiLibrary.Data.Repository;
public class WebApiDtoRepository : IWebApiDtoRepository, IDisposable {
    private readonly WebApiDbContext _context;
    private readonly IMapper _mapper;

    public WebApiDtoRepository(IDbContextFactory<WebApiDbContext> factory) {
        _context = factory.CreateDbContext();
        _mapper =
            new Mapper(
                new MapperConfiguration(cfg => {
                    cfg.CreateMap<Ruolo, RuoloDTO>();
                    cfg.CreateMap<RuoloDTO, Ruolo>();

                    cfg.CreateMap<CreaRuoloDTO, Ruolo>();
                    cfg.CreateMap<AggiornaRuoloDTO, Ruolo>();
                    
                    cfg.CreateMap<Utente, UtenteDTO>();
                    cfg.CreateMap<UtenteDTO, Utente>();

                    cfg.CreateMap<CreaUtenteDTO, Utente>();
                    cfg.CreateMap<AggiornaUtenteDTO, Utente>();
                }));
    }

    public WebApiDtoRepository(IDbContextFactory<WebApiDbContext> factory, IMapper mapper) {
        _context = factory.CreateDbContext();
        _mapper = mapper;
    }

    public bool SaveChanges() => _context.SaveChanges() >= 0;
    public void Dispose() => _context.Dispose();

    #region Ruolo

    public RuoloDTO Create(CreaRuoloDTO creaRuolo) {
        if (TryReadFromPrimaryKey(creaRuolo, out Ruolo? Ruolo))
            throw new Exception($"Ruolo '{creaRuolo.Nome}' già censito");

        Ruolo = _mapper.Map<Ruolo>(creaRuolo);

        _context.Add(Ruolo);

        SaveChanges();

        return _mapper.Map<RuoloDTO>(Ruolo!);
    }

    public List<RuoloDTO> Read(RicercaRuoloDTO ricercaRuolo) {
        IQueryable<Ruolo> IQueryableRuolo = _context.ListaRuoli;

        if (ricercaRuolo.Nome is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Nome.Contains(ricercaRuolo.Nome));

        if (ricercaRuolo.Descrizione is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Descrizione.Contains(ricercaRuolo.Descrizione));

        if (ricercaRuolo.Categoria is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Categoria.Contains(ricercaRuolo.Categoria));

        var list = IQueryableRuolo.ToList();

        return list.Select(p => _mapper.Map<RuoloDTO>(p)).ToList();
    }
    
    public RuoloDTO Update(AggiornaRuoloDTO aggiornaRuolo) {
        Ruolo Ruolo = ReadFromPrimaryKey(aggiornaRuolo);

        _mapper.Map(aggiornaRuolo, Ruolo);

        SaveChanges();

        return _mapper.Map<RuoloDTO>(Ruolo);
    }

    public void Delete(EliminaRuoloDTO eliminaRuolo) {
        _context.Remove(ReadFromPrimaryKey(eliminaRuolo));

        SaveChanges();
    }

    private bool TryReadFromPrimaryKey(RuoloPrimaryKey RuoloPrimaryKey, out Ruolo? Ruolo) {
        Ruolo = _context.ListaRuoli.SingleOrDefault(p => p.Nome == RuoloPrimaryKey.Nome);

        return Ruolo != null;
    }

    private Ruolo ReadFromPrimaryKey(RuoloPrimaryKey RuoloPrimaryKey) {
        if (!TryReadFromPrimaryKey(RuoloPrimaryKey, out Ruolo? Ruolo))
            throw new Exception($"Ruolo '{RuoloPrimaryKey.Nome}' non trovato");

        return Ruolo!;
    }

    #endregion

    #region Utente

    public UtenteDTO Create(CreaUtenteDTO creaUtente) {
        if (TryReadFromPrimaryKey(creaUtente, out Utente? utente))
            throw new Exception($"Utente '{utente!.NomeUtente}' già censito");

        utente = _mapper.Map<Utente>(creaUtente);
        utente.DataCreazione = DateTime.Now;

        ImpostaPassword(ref utente, creaUtente.Password);

        _context.Add(utente);

        SaveChanges();

        return _mapper.Map<UtenteDTO>(utente!);
    }

    public List<UtenteDTO> Read(RicercaUtenteDTO ricercaUtente) {
        IQueryable<Utente> IQueryableUtente = _context.ListaUtenti;

        if (ricercaUtente.NomeUtente is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.NomeUtente == ricercaUtente.NomeUtente);

        if (ricercaUtente.Nome is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.Nome == ricercaUtente.Nome);

        if (ricercaUtente.Cognome is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.Cognome == ricercaUtente.Cognome);

        if (ricercaUtente.Email is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.Email == ricercaUtente.Email);

        if (ricercaUtente.NomeRuolo is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.NomeRuolo == ricercaUtente.NomeRuolo);

        var list = IQueryableUtente.ToList();

        return list.Select(p => _mapper.Map<UtenteDTO>(p)).ToList();
    }

    public UtenteDTO Update(AggiornaUtenteDTO aggiornaUtente) {
        Utente utente = ReadFromPrimaryKey(aggiornaUtente);

        _mapper.Map(aggiornaUtente, utente);

        SaveChanges();

        return _mapper.Map<UtenteDTO>(utente);
    }

    public void Delete(EliminaUtenteDTO eliminaUtente) {
        _context.Remove(ReadFromPrimaryKey(eliminaUtente));

        SaveChanges();
    }

    public void AggiornaPassword(AggiornaPasswordDTO aggiornaPassword) {
        Utente utente = ReadFromPrimaryKey(aggiornaPassword);

        ImpostaPassword(ref utente, aggiornaPassword.Password);

        SaveChanges();
    }

    public bool VerificaPassword(VerificaPasswordDTO verificaPassword) {
        Utente utente = ReadFromPrimaryKey(verificaPassword);

        return utente.Password == GetHash(verificaPassword.Password, utente.Salt);
    }

    private Utente ImpostaPassword(ref Utente utente, string password) {
        utente.Password = GetHash(password, out string salt);
        utente.Salt = salt;
        utente.DataUltimoCambioPassword = DateTime.Now;

        return utente;
    }

    private bool TryReadFromPrimaryKey(UtentePrimaryKey utentePrimaryKey, out Utente? utente) {
        utente = _context.ListaUtenti.SingleOrDefault(p => p.NomeUtente == utentePrimaryKey.NomeUtente);

        return utente != null;
    }

    private Utente ReadFromPrimaryKey(UtentePrimaryKey utentePrimaryKey) {
        if (!TryReadFromPrimaryKey(utentePrimaryKey, out Utente? utente))
            throw new Exception($"Utente '{utentePrimaryKey.NomeUtente}' non trovato");

        return utente!;
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


