namespace NewVersion_EP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_Usuario
    {
        public TB_Usuario()
        {
            TB_PerguntasVendedor = new HashSet<TB_PerguntasVendedor>();
            TB_Servicos = new HashSet<TB_Servicos>();
            TB_ServicosVendidos = new HashSet<TB_ServicosVendidos>();
        }

        public int Id { get; set; }

        public int SexoId { get; set; }

        public Guid UserGuid { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; }

        [Required]
        [StringLength(11)]
        public string Telefone { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        public DateTime DtCriacao { get; set; }

        public DateTime? DtAlteracao { get; set; }

        public DateTime? DtUltimoAcesso { get; set; }

        public int? ContaErro { get; set; }

        public bool? isLocked { get; set; }

        public bool isAtivo { get; set; }

        public virtual ICollection<TB_PerguntasVendedor> TB_PerguntasVendedor { get; set; }

        public virtual ICollection<TB_Servicos> TB_Servicos { get; set; }

        public virtual ICollection<TB_ServicosVendidos> TB_ServicosVendidos { get; set; }

        public virtual TB_Sexo TB_Sexo { get; set; }
    }
}
