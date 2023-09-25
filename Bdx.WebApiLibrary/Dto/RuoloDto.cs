using System.ComponentModel.DataAnnotations;

namespace Bdx.WebApiLibrary.Dto;

public class RuoloDto {
    public string Nome { get; set; } = null!;

    public string Descrizione { get; set; } = null!;

    public string Categoria { get; set; } = null!;
}

public class CreaRuoloDto : RuoloPrimaryKey {
    [Required]
    public string Descrizione { get; set; } = null!;

    [Required]
    public string Categoria { get; set; } = null!;
}

public class RicercaRuoloDto {
    public string? Nome { get; set; }

    public string? Descrizione { get; set; }

    public string? Categoria { get; set; }
}

public class AggiornaRuoloDto : RuoloPrimaryKey {
    public string Descrizione { get; set; } = null!;

    public string Categoria { get; set; } = null!;
}

public class EliminaRuoloDto : RuoloPrimaryKey {

}

public abstract class RuoloPrimaryKey {
    [Required]
    public string Nome { get; set; } = null!;
}

