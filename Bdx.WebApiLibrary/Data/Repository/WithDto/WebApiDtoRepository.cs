using AutoMapper;
using Bdx.WebApiLibrary.Dto;
using Bdx.WebApiLibrary.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Bdx.WebApiLibrary.Data.Repository;

public class WebApiDtoRepository : IWebApiDtoRepository, IDisposable {
    private readonly WebApiDbContext _context;
    private readonly IMapper _mapper;

    private bool _disposed;

    public WebApiDtoRepository(IDbContextFactory<WebApiDbContext> factory) {
        _context = factory.CreateDbContext();
        _mapper = new Mapper(
            new MapperConfiguration(cfg => {
                cfg.CreateMap<Ruolo, RuoloDto>();
                cfg.CreateMap<RuoloDto, Ruolo>();

                cfg.CreateMap<CreaRuoloDto, Ruolo>();
                cfg.CreateMap<AggiornaRuoloDto, Ruolo>();
                    
                cfg.CreateMap<Utente, UtenteDto>();
                cfg.CreateMap<UtenteDto, Utente>();

                cfg.CreateMap<CreaUtenteDto, Utente>();
                cfg.CreateMap<AggiornaUtenteDto, Utente>();
            }));
    }

    public WebApiDtoRepository(IDbContextFactory<WebApiDbContext> factory, IMapper mapper) {
        _context = factory.CreateDbContext();
        _mapper = mapper;
    }

    public bool SaveChanges() => _context.SaveChanges() >= 0;

    protected virtual void Dispose(bool disposing) {
        if (!_disposed) {
            if (disposing) {
                _context.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #region Ruolo

    public RuoloDto Create(CreaRuoloDto creaRuolo) {
        if (TryReadFromPrimaryKey(creaRuolo, out Ruolo? Ruolo))
            throw new Exception($"Ruolo '{creaRuolo.Nome}' già censito");

        Ruolo = _mapper.Map<Ruolo>(creaRuolo);

        _context.Add(Ruolo);

        SaveChanges();

        return _mapper.Map<RuoloDto>(Ruolo!);
    }

    public List<RuoloDto> Read(RicercaRuoloDto ricercaRuolo) {
        IQueryable<Ruolo> IQueryableRuolo = _context.ListaRuoli;

        if (ricercaRuolo.Nome is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Nome.Contains(ricercaRuolo.Nome));

        if (ricercaRuolo.Descrizione is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Descrizione.Contains(ricercaRuolo.Descrizione));

        if (ricercaRuolo.Categoria is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Categoria.Contains(ricercaRuolo.Categoria));

        var list = IQueryableRuolo.ToList();

        return list.Select(p => _mapper.Map<RuoloDto>(p)).ToList();
    }
    
    public RuoloDto Update(AggiornaRuoloDto aggiornaRuolo) {
        Ruolo Ruolo = ReadFromPrimaryKey(aggiornaRuolo);

        _mapper.Map(aggiornaRuolo, Ruolo);

        SaveChanges();

        return _mapper.Map<RuoloDto>(Ruolo);
    }

    public void Delete(EliminaRuoloDto eliminaRuolo) {
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

    public UtenteDto Create(CreaUtenteDto creaUtente) {
        if (TryReadFromPrimaryKey(creaUtente, out Utente? utente))
            throw new Exception($"Utente '{utente!.NomeUtente}' già censito");

        utente = _mapper.Map<Utente>(creaUtente);
        utente.DataCreazione = DateTime.Now;

        ImpostaPassword(ref utente, creaUtente.Password);

        _context.Add(utente);

        SaveChanges();

        return _mapper.Map<UtenteDto>(utente!);
    }

    public List<UtenteDto> Read(RicercaUtenteDto ricercaUtente) {
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

        return list.Select(p => _mapper.Map<UtenteDto>(p)).ToList();
    }

    public UtenteDto Update(AggiornaUtenteDto aggiornaUtente) {
        Utente utente = ReadFromPrimaryKey(aggiornaUtente);

        _mapper.Map(aggiornaUtente, utente);

        SaveChanges();

        return _mapper.Map<UtenteDto>(utente);
    }

    public void Delete(EliminaUtenteDto eliminaUtente) {
        _context.Remove(ReadFromPrimaryKey(eliminaUtente));

        SaveChanges();
    }

    public void AggiornaPassword(AggiornaPasswordDto aggiornaPassword) {
        Utente utente = ReadFromPrimaryKey(aggiornaPassword);

        ImpostaPassword(ref utente, aggiornaPassword.Password);

        SaveChanges();
    }

    public bool VerificaPassword(VerificaPasswordDto verificaPassword) {
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


