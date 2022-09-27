﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDX.WebApiLibrary.Model;

[Table("Accessi", Schema = "Bdx")]
public class Accesso {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string NomeUtente { get; set; } = string.Empty;

    public DateTime Data { get; set; }

    public Utente? Utente { get; set; } = null!;
}