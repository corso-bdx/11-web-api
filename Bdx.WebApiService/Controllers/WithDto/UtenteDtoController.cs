using Bdx.WebApiLibrary.Data.Repository;
using Bdx.WebApiLibrary.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Bdx.WebApiService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UtenteDtoController : ControllerBase {
    private readonly IWebApiDtoRepository _repository;

    public UtenteDtoController(IWebApiDtoRepository repository) {
        _repository = repository;
    }

	[HttpPost(Name = "CreateUser")]
	public ActionResult<UtenteDto> CreateUser(CreaUtenteDto creaUtente) {
		try {
			return Ok(_repository.Create(creaUtente));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "ReadUser")]
	public ActionResult<List<UtenteDto>> ReadUser([FromQuery] RicercaUtenteDto ricercaUtente) {
		try {
			return Ok(_repository.Read(ricercaUtente));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "UpdateUser")]
	public ActionResult<UtenteDto> UpdateUser(AggiornaUtenteDto aggiornaUtente) {
		try {
			return Ok(_repository.Update(aggiornaUtente));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete(Name = "DeleteUser")]
	public ActionResult DeleteUser(EliminaUtenteDto eliminaUtente) {
		try {
			_repository.Delete(eliminaUtente);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "UpdatePassword")]
	public ActionResult UpdatePassword(AggiornaPasswordDto aggiornaPassword) {
		try {
			_repository.AggiornaPassword(aggiornaPassword);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "CheckPassword")]
	public ActionResult<bool> CheckPassword([FromQuery] VerificaPasswordDto verificaPassword) {
		try {
			return Ok(_repository.VerificaPassword(verificaPassword));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}
}

