using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewVersion_EP.Models.DTO
{
    public class Servico
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int CategoriaId { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string Foto { get; set; }

        public int TempoEntrega { get; set; }

        public string Tag { get; set; }

        public string Video { get; set; }

        public string Instrucoes { get; set; }

        public bool TemArquivo { get; set; }

        public DateTime? DtAtivacao { get; set; }

        public bool isAtivo { get; set; }

        public Categoria Categoria { get; set; }

        public List<PerguntasVendedor> PerguntasVendedor { get; set; }

        public Usuario Usuario { get; set; }
    }
} 