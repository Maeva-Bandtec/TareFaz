namespace NewVersion_EP.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MaevaDBContext : DbContext
    {
        public MaevaDBContext()
            : base("name=MaevaDBContext")
        {
        }

        public virtual DbSet<TB_Categoria> TB_Categoria { get; set; }
        public virtual DbSet<TB_FaleConosco> TB_FaleConosco { get; set; }
        public virtual DbSet<TB_FaleConoscoAssunto> TB_FaleConoscoAssunto { get; set; }
        public virtual DbSet<TB_PerguntasVendedor> TB_PerguntasVendedor { get; set; }
        public virtual DbSet<TB_Servicos> TB_Servicos { get; set; }
        public virtual DbSet<TB_ServicosVendidos> TB_ServicosVendidos { get; set; }
        public virtual DbSet<TB_Sexo> TB_Sexo { get; set; }
        public virtual DbSet<TB_Usuario> TB_Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TB_Categoria>()
                .Property(e => e.NomeCategoria)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Categoria>()
                .HasMany(e => e.TB_Servicos)
                .WithRequired(e => e.TB_Categoria)
                .HasForeignKey(e => e.CategoriaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_FaleConosco>()
                .Property(e => e.NomeRemetente)
                .IsUnicode(false);

            modelBuilder.Entity<TB_FaleConosco>()
                .Property(e => e.EmailRemetente)
                .IsUnicode(false);

            modelBuilder.Entity<TB_FaleConosco>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TB_FaleConoscoAssunto>()
                .Property(e => e.Assunto)
                .IsUnicode(false);

            modelBuilder.Entity<TB_FaleConoscoAssunto>()
                .HasMany(e => e.TB_FaleConosco)
                .WithRequired(e => e.TB_FaleConoscoAssunto)
                .HasForeignKey(e => e.AssuntoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_PerguntasVendedor>()
                .Property(e => e.Pergunta)
                .IsUnicode(false);

            modelBuilder.Entity<TB_PerguntasVendedor>()
                .Property(e => e.Resposta)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Servicos>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Servicos>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Servicos>()
                .Property(e => e.Foto)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Servicos>()
                .Property(e => e.Tag)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Servicos>()
                .Property(e => e.Video)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Servicos>()
                .Property(e => e.Instrucoes)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Servicos>()
                .HasMany(e => e.TB_PerguntasVendedor)
                .WithRequired(e => e.TB_Servicos)
                .HasForeignKey(e => e.ServicoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_Servicos>()
                .HasMany(e => e.TB_ServicosVendidos)
                .WithRequired(e => e.TB_Servicos)
                .HasForeignKey(e => e.ServicoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_ServicosVendidos>()
                .Property(e => e.CaminhoArquivo)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Sexo>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Sexo>()
                .HasMany(e => e.TB_Usuario)
                .WithRequired(e => e.TB_Sexo)
                .HasForeignKey(e => e.SexoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_Usuario>()
                .Property(e => e.NomeCompleto)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Usuario>()
                .Property(e => e.Telefone)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Usuario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Usuario>()
                .Property(e => e.Senha)
                .IsUnicode(false);

            modelBuilder.Entity<TB_Usuario>()
                .HasMany(e => e.TB_PerguntasVendedor)
                .WithRequired(e => e.TB_Usuario)
                .HasForeignKey(e => e.CompradorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_Usuario>()
                .HasMany(e => e.TB_Servicos)
                .WithRequired(e => e.TB_Usuario)
                .HasForeignKey(e => e.UsuarioId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_Usuario>()
                .HasMany(e => e.TB_ServicosVendidos)
                .WithRequired(e => e.TB_Usuario)
                .HasForeignKey(e => e.CompradorId)
                .WillCascadeOnDelete(false);
        }
    }
}
