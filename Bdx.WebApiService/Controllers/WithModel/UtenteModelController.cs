using Bdx.WebApiLibrary.Data.Repository;
using Bdx.WebApiLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bdx.WebApiService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UtenteModelController : ControllerBase {
    private readonly IWebApiModelRepository _repository;

    public UtenteModelController(IWebApiModelRepository repository) {
        _repository = repository;
    }

	[HttpGet(Name = "ElencoUtenti")]
	public ActionResult<List<Utente>> ElencoUtenti() {
		try {
			return Ok(_repository.ElencoUtenti());
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "CercaUtente")]
	public ActionResult<Utente> CercaUtente(string nomeUtente) {
		try {
			Utente? result = _repository.CercaUtente(nomeUtente);
			if (result is null) 
				return NotFound();

			return Ok(result);
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPost(Name = "InserisciUtente")]
	public ActionResult InserisciUtente(Utente utenteDaCreare) {
		try {
			_repository.InserisciUtente(utenteDaCreare);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "ModificaUtente")]
	public ActionResult ModificaUtente(Utente utenteDaModificare) {
		try {
			_repository.ModificaUtente(utenteDaModificare);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete(Name = "EliminaUtente")]
	public ActionResult EliminaUtente(string nomeUtente) {
		try {
			_repository.EliminaUtente(nomeUtente);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "AggiornaPassword")]
	public ActionResult AggiornaPassword(string nomeUtente, string password) {
		try {
			_repository.AggiornaPassword(nomeUtente, password);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "VerificaPassword")]
	public ActionResult<bool> VerificaPassword(string nomeUtente, string password) {
		try {
			return Ok(_repository.VerificaPassword(nomeUtente, password));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}


}

