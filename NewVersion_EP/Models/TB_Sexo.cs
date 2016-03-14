namespace NewVersion_EP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_Sexo
    {
        public TB_Sexo()
        {
            TB_Usuario = new HashSet<TB_Usuario>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(6)]
        public string Descricao { get; set; }

        public virtual ICollection<TB_Usuario> TB_Usuario { get; set; }
    }
}
