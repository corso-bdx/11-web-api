using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace BDX.WebApiLibrary.DTO;
public class UtenteDTO {
    public string NomeUtente { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string NomeRuolo { get; set; } = null!;

    public DateTime DataCreazione { get; set; }

    public DateTime DataUltimoCambioPassword { get; set; }
}

public class CreaUtenteDTO : UtentePrimaryKey {
    [Required]
    public string Nome { get; set; } = null!;

    [Required]
    public string Cognome { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    [RegexStringValidator(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfermaPassword { get; set; } = null!;

    public string NomeRuolo { get; set; } = null!;
}

public class RicercaUtenteDTO {
    public string? NomeUtente { get; set; }

    public string? Nome { get; set; }

    public string? Cognome { get; set; }

    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public string? NomeRuolo { get; set; }
}

public class AggiornaUtenteDTO : UtentePrimaryKey {
    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [RegexStringValidator(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    public string Email { get; set; } = null!;

    public string NomeRuolo { get; set; } = null!;
}

public class AggiornaPasswordDTO : UtentePrimaryKey {
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfermaPassword { get; set; } = null!;
}

public class VerificaPasswordDTO : UtentePrimaryKey {
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}

public class EliminaUtenteDTO : UtentePrimaryKey {

}

public abstract class UtentePrimaryKey {
    [Required]
    public string NomeUtente { get; set; } = null!;
}
