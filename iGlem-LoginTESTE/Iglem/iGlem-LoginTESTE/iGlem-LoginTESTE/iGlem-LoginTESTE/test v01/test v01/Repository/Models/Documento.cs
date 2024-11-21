using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace test_v01.Repository.Models;

[Table("documento")]
public partial class Documento
{
    [Key]
    [Column("documentoid")]
    public int Documentoid { get; set; }

    [Column("caminhodocumento")]
    [Unicode(false)]
    public string? Caminhodocumento { get; set; }

    [Column("documentonome")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Documentonome { get; set; }

    [Column("idusuario")]
    public int? Idusuario { get; set; }

    [Column("conteudodocumento")]
    public string? conteudodocumento { get; set; }

    [Column("datacriacao")]
    public DateTime datacriacao { get; set; }


    [Column("IsFavorite")]
    public bool IsFavorite { get; set; }

    public byte[]? FileData { get; set; }

    [ForeignKey("Idusuario")]
    [InverseProperty("Documentos")]
    public virtual Usuario? IdusuarioNavigation { get; set; }

    [ForeignKey("IdDocumento")]
    [InverseProperty("IdDocumentos")]
    public virtual ICollection<PalavraChave> IdPalavraChaves { get; set; } = new List<PalavraChave>();
}
