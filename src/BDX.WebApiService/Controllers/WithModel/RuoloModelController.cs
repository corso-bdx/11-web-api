using BDX.WebApiLibrary.Data.Repository;
using BDX.WebApiLibrary.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BDX.WebApiService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RuoloModelController : ControllerBase {
    private readonly IWebApiModelRepository _repository;

    public RuoloModelController(IWebApiModelRepository repository) {
        _repository = repository;
    }

	[HttpGet(Name = "ElencoRuoli")]
	public ActionResult<List<Ruolo>> ElencoRuoli() {
		try {
			return Ok(_repository.ElencoRuoli());
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "CercaRuolo")]
	public ActionResult<Ruolo> CercaRuolo(string nomeRuolo) {
		try {
			Ruolo? result = _repository.CercaRuolo(nomeRuolo);
			if (result is null) 
				return NotFound();

			return Ok(result);
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPost(Name = "InserisciRuolo")]
	public ActionResult InserisciRuolo(Ruolo ruoloDaCreare) {
		try {
			_repository.InserisciRuolo(ruoloDaCreare);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPut(Name = "ModificaRuolo")]
	public ActionResult ModificaRuolo(Ruolo ruoloDaModificare) {
		try {
			_repository.ModificaRuolo(ruoloDaModificare);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete(Name = "EliminaRuolo")]
	public ActionResult EliminaRuolo(string nomeRuolo) {
		try {
			_repository.EliminaRuolo(nomeRuolo);
			return Ok();
		} catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

}

