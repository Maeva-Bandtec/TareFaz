namespace NewVersion_EP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_FaleConoscoAssunto
    {
        public TB_FaleConoscoAssunto()
        {
            TB_FaleConosco = new HashSet<TB_FaleConosco>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Assunto { get; set; }

        public int Ordem { get; set; }

        public virtual ICollection<TB_FaleConosco> TB_FaleConosco { get; set; }
    }
}
