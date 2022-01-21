using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos.Entity
{
    public class ComponenteCurricularEntityMap : IEntityTypeConfiguration<ComponenteCurricular>
    {
        public void Configure(EntityTypeBuilder<ComponenteCurricular> builder)
        {
            builder.
            builder.ToTable("componente_curricular");

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.CodigoDre)
                .HasColumnType("varchar")
                .HasColumnName("dre_id")
                .IsRequired();

            builder.Property(c => c.Abreviacao)
                .HasColumnType("varchar")
                .HasColumnName("abreviacao")
                .IsRequired();

            builder.Property(c => c.Nome)
                .HasColumnType("varchar")
                .HasColumnName("nome")
                .IsRequired();

            builder.Property(c => c.DataAtualizacao)
                .HasColumnType("timestamp")
                .HasColumnName("data_atualizacao")
                .IsRequired();

        }
    }
}