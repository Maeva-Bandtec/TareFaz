namespace NewVersion_EP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_Categoria
    {
        public TB_Categoria()
        {
            TB_Servicos = new HashSet<TB_Servicos>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string NomeCategoria { get; set; }

        public virtual ICollection<TB_Servicos> TB_Servicos { get; set; }
    }
}
