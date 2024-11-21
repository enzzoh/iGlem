using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace test_v01.Repository.Models;

[Table("palavra_chave")]
public partial class PalavraChave
{
    [Key]
    [Column("id_palavra_chave")]
    public int IdPalavraChave { get; set; }

    [Column("palavra")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Palavra { get; set; }

    [ForeignKey("IdPalavraChave")]
    [InverseProperty("IdPalavraChaves")]
    public virtual ICollection<Documento> IdDocumentos { get; set; } = new List<Documento>();

    [ForeignKey("IdPalavraChave")]
    [InverseProperty("IdPalavraChaves")]
    public virtual ICollection<Localizacao> IdLocalizacaos { get; set; } = new List<Localizacao>();
}
