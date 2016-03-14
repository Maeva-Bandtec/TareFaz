namespace NewVersion_EP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_PerguntasVendedor
    {
        public int Id { get; set; }

        public int ServicoId { get; set; }

        public int CompradorId { get; set; }

        [Required]
        public string Pergunta { get; set; }

        public DateTime? DtPergunta { get; set; }

        public string Resposta { get; set; }

        public DateTime? DtResposta { get; set; }

        public bool isAtivo { get; set; }

        public virtual TB_Usuario TB_Usuario { get; set; }

        public virtual TB_Servicos TB_Servicos { get; set; }
    }
}
