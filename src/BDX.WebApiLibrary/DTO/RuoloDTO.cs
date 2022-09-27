using System.ComponentModel.DataAnnotations;

namespace BDX.WebApiLibrary.DTO;
public class RuoloDTO {
    public string Nome { get; set; } = null!;

    public string Descrizione { get; set; } = null!;

    public string Categoria { get; set; } = null!;
}

public class CreaRuoloDTO : RuoloPrimaryKey {
    [Required]
    public string Descrizione { get; set; } = null!;

    [Required]
    public string Categoria { get; set; } = null!;
}

public class RicercaRuoloDTO {
    public string? Nome { get; set; }

    public string? Descrizione { get; set; }

    public string? Categoria { get; set; }
}

public class AggiornaRuoloDTO : RuoloPrimaryKey {
    public string Descrizione { get; set; } = null!;

    public string Categoria { get; set; } = null!;
}

public class EliminaRuoloDTO : RuoloPrimaryKey {

}

public abstract class RuoloPrimaryKey {
    [Required]
    public string Nome { get; set; } = null!;
}

