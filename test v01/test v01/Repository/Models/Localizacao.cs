using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace test_v01.Repository.Models;

[Table("localizacao")]
public partial class Localizacao
{
    [Key]
    [Column("id_localizacao")]
    public int IdLocalizacao { get; set; }

    [Column("paragrafo")]
    public int? Paragrafo { get; set; }

    [Column("pagina")]
    public int? Pagina { get; set; }

    [ForeignKey("IdLocalizacao")]
    [InverseProperty("IdLocalizacaos")]
    public virtual ICollection<PalavraChave> IdPalavraChaves { get; set; } = new List<PalavraChave>();
}
