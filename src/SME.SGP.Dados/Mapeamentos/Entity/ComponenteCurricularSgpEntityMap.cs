using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos.Entity
{
    public class ComponenteCurricularSgpEntityMap : IEntityTypeConfiguration<ComponenteCurricularSgp>
    {
        public void Configure(EntityTypeBuilder<ComponenteCurricularSgp> builder)
        {
            builder.ToTable("componente_curricular");

            builder.Property(c => c.Id).HasColumnName("id");
            builder.Property(c => c.AreaConhecimentoId).HasColumnName("area_conhecimento_id");
            builder.Property(c => c.ComponenteCurricularPaiId).HasColumnName("componente_curricular_pai_id");
            builder.Property(c => c.EhBaseNacional).HasColumnName("eh_base_nacional");
            builder.Property(c => c.EhCompatilhada).HasColumnName("eh_compartilhada");
            builder.Property(c => c.EhRegenciaClasse).HasColumnName("eh_regencia");
            builder.Property(c => c.EhTerritorio).HasColumnName("eh_territorio");
            builder.Property(c => c.GrupoMatrizId).HasColumnName("grupo_matriz_id");
            builder.Property(c => c.PermiteLancamentoNota).HasColumnName("permite_lancamento_nota");
            builder.Property(c => c.PermiteRegistroFrequencia).HasColumnName("permite_registro_frequencia");
            builder.Property(c => c.DescricaoSGP).HasColumnName("descricaoSGP");
            builder.Property(c => c.DescricaoInfantil).HasColumnName("descricao_infantil");
        }
    }
}