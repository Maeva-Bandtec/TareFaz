namespace NewVersion_EP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_Servicos
    {
        public TB_Servicos()
        {
            TB_PerguntasVendedor = new HashSet<TB_PerguntasVendedor>();
            TB_ServicosVendidos = new HashSet<TB_ServicosVendidos>();
        }

        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int CategoriaId { get; set; }

        [Required]
        [StringLength(20)]
        public string Titulo { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        [StringLength(100)]
        public string Foto { get; set; }

        public int TempoEntrega { get; set; }

        [StringLength(50)]
        public string Tag { get; set; }

        public string Video { get; set; }

        [Required]
        public string Instrucoes { get; set; }

        public bool TemArquivo { get; set; }

        public DateTime? DtAtivacao { get; set; }

        public bool isAtivo { get; set; }

        public virtual TB_Categoria TB_Categoria { get; set; }

        public virtual ICollection<TB_PerguntasVendedor> TB_PerguntasVendedor { get; set; }

        public virtual TB_Usuario TB_Usuario { get; set; }

        public virtual ICollection<TB_ServicosVendidos> TB_ServicosVendidos { get; set; }

        [NotMapped]
        public int TotalServicos { get; set; }
    }
}
