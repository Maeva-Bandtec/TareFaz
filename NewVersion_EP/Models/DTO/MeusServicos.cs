using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewVersion_EP.Models.DTO
{
    public class MeusServicos
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public int QtdVendidos { get; set; }

        public int QtdEntregues { get; set; }

        public int QtdPendentes { get; set; }

        public bool IsAtivo { get; set; }
    }
}