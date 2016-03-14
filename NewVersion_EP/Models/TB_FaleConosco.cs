namespace NewVersion_EP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_FaleConosco
    {
        public int Id { get; set; }

        public int AssuntoId { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeRemetente { get; set; }

        [Required]
        [StringLength(50)]
        public string EmailRemetente { get; set; }

        [Required]
        public string Descricao { get; set; }

        public DateTime? DtCriacao { get; set; }

        public bool isAtivo { get; set; }

        public virtual TB_FaleConoscoAssunto TB_FaleConoscoAssunto { get; set; }
    }
}
