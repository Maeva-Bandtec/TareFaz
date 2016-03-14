using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewVersion_EP.Models.DTO
{
    public class Usuario
    {
        public int Id { get; set; }

        public int SexoId { get; set; }

        public Guid UserGuid { get; set; }

        public string NomeCompleto { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public DateTime DtCriacao { get; set; }

        public DateTime DtAlteracao { get; set; }

        public DateTime DtUltimoAcesso { get; set; }

        public int ContaErro { get; set; }

        public bool isLocked { get; set; }

        public bool isAtivo { get; set; }
    }
}