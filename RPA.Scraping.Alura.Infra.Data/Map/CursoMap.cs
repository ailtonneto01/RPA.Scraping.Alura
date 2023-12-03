using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPA.Scraping.Alura.Domain.Entities;

namespace RPA.Scraping.Alura.Infra.Data.Map
{
    public class CursoMap : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("cursos");
            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.UrlCurso);

            builder.Property(x => x.TermoPesquisado)
                .HasConversion(x => x.ToString(), x => x)
                .IsRequired()
                .HasColumnName("termo_pesquisado")
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Titulo)
                .HasConversion(x => x.ToString(), x => x)
                .IsRequired()
                .HasColumnName("titulo")
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Professor)
                .HasConversion(x => x.ToString(), x => x)
                .HasColumnName("professor")
                .HasColumnType("varchar(200)");

            builder.Property(x => x.CargaHoraria)
                .HasConversion(x => x.ToString(), x => x)
                .HasColumnName("carga_horaria")
                .HasColumnType("varchar(10)");

            builder.Property(x => x.Descricao)
                .IsRequired()
                .HasConversion(x => x.ToString(), x => x)
                .HasColumnName("descricao")
                .HasColumnType("varchar(800)");

            builder.Property(x => x.LastUpdate)
                .IsRequired()
                .HasConversion(x => x.ToString(), x => x)
                .HasColumnName("lastupdate")
                .HasColumnType("varchar(20)");            
        }
    }
}
