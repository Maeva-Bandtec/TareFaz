using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewVersion_EP.Models.DTO
{
    public class PerguntasVendedor
    {
        public int Id { get; set; }

        public int ServicoId { get; set; }

        public int CompradorId { get; set; }

        public string Pergunta { get; set; }

        public DateTime? DtPergunta { get; set; }

        public string Resposta { get; set; }

        public DateTime? DtResposta { get; set; }

        public bool isAtivo { get; set; }

        public Usuario Usuario { get; set; }
    }
}