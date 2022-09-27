using BDX.WebApiLibrary.Data.Repository;
using BDX.WebApiLibrary.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BDX.WebApiService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RuoloDtoController : ControllerBase {
    private readonly IWebApiDtoRepository _repository;

    public RuoloDtoController(IWebApiDtoRepository repository) {
        _repository = repository;
    }

	[HttpPost(Name = "CreateRole")]
	public ActionResult<RuoloDTO> CreateRole(CreaRuoloDTO creaRuolo) {
		try {
			return Ok(_repository.Create(creaRuolo));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "ReadRole")]
	public ActionResult<List<RuoloDTO>> ReadRole([FromQuery] RicercaRuoloDTO ricercaRuolo) {
		try {
			return Ok(_repository.Read(ricercaRuolo));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "UpdateRole")]
	public ActionResult<RuoloDTO> UpdateRole(AggiornaRuoloDTO aggiornaRuolo) {
		try {
			return Ok(_repository.Update(aggiornaRuolo));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete(Name = "DeleteRole")]
	public ActionResult DeleteRole(EliminaRuoloDTO eliminaRuolo) {
		try {
			_repository.Delete(eliminaRuolo);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}
}

