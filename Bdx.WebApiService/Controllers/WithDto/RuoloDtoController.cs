using Bdx.WebApiLibrary.Data.Repository;
using Bdx.WebApiLibrary.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Bdx.WebApiService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RuoloDtoController : ControllerBase {
    private readonly IWebApiDtoRepository _repository;

    public RuoloDtoController(IWebApiDtoRepository repository) {
        _repository = repository;
    }

	[HttpPost(Name = "CreateRole")]
	public ActionResult<RuoloDto> CreateRole(CreaRuoloDto creaRuolo) {
		try {
			return Ok(_repository.Create(creaRuolo));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "ReadRole")]
	public ActionResult<List<RuoloDto>> ReadRole([FromQuery] RicercaRuoloDto ricercaRuolo) {
		try {
			return Ok(_repository.Read(ricercaRuolo));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "UpdateRole")]
	public ActionResult<RuoloDto> UpdateRole(AggiornaRuoloDto aggiornaRuolo) {
		try {
			return Ok(_repository.Update(aggiornaRuolo));
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete(Name = "DeleteRole")]
	public ActionResult DeleteRole(EliminaRuoloDto eliminaRuolo) {
		try {
			_repository.Delete(eliminaRuolo);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}
}

