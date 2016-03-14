using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewVersion_EP.Models.DTO
{
    public class ServicoVendido
    {
        public int Id { get; set; }

        public int ServicoId { get; set; }

        public int CompradorId { get; set; }

        public DateTime? DtPedido { get; set; }

        public DateTime? DtEntrega { get; set; }

        public string CaminhoArquivo { get; set; }

        public bool isAtivo { get; set; }

        public Servico Servico { get; set; }

        public Usuario Usuario { get; set; }
    }
}