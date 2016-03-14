namespace NewVersion_EP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_ServicosVendidos
    {
        public int Id { get; set; }

        public int ServicoId { get; set; }

        public int CompradorId { get; set; }

        public DateTime? DtPedido { get; set; }

        public DateTime? DtEntrega { get; set; }

        [StringLength(100)]
        public string CaminhoArquivo { get; set; }

        public bool isAtivo { get; set; }

        public virtual TB_Servicos TB_Servicos { get; set; }

        public virtual TB_Usuario TB_Usuario { get; set; }
    }
}
