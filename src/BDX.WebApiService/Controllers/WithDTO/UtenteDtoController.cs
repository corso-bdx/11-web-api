using BDX.WebApiLibrary.Data.Repository;
using BDX.WebApiLibrary.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BDX.WebApiService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UtenteDtoController : ControllerBase {
    private readonly IWebApiDtoRepository _repository;

    public UtenteDtoController(IWebApiDtoRepository repository) {
        _repository = repository;
    }

	[HttpPost(Name = "CreateUser")]
	public ActionResult<UtenteDTO> CreateUser(CreaUtenteDTO creaUtente) {
		try {
			return Ok(_repository.Create(creaUtente));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "ReadUser")]
	public ActionResult<List<UtenteDTO>> ReadUser([FromQuery] RicercaUtenteDTO ricercaUtente) {
		try {
			return Ok(_repository.Read(ricercaUtente));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "UpdateUser")]
	public ActionResult<UtenteDTO> UpdateUser(AggiornaUtenteDTO aggiornaUtente) {
		try {
			return Ok(_repository.Update(aggiornaUtente));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete(Name = "DeleteUser")]
	public ActionResult DeleteUser(EliminaUtenteDTO eliminaUtente) {
		try {
			_repository.Delete(eliminaUtente);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "UpdatePassword")]
	public ActionResult UpdatePassword(AggiornaPasswordDTO aggiornaPassword) {
		try {
			_repository.AggiornaPassword(aggiornaPassword);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "CheckPassword")]
	public ActionResult<bool> CheckPassword([FromQuery] VerificaPasswordDTO verificaPassword) {
		try {
			return Ok(_repository.VerificaPassword(verificaPassword));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}
}

