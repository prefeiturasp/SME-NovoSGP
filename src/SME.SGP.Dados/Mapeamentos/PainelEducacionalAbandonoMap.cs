using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalAbandonoMap : SimpleEntityMap<PainelEducacionalAbandono>
    {
        public PainelEducacionalAbandonoMap()
        {
            ToTable("painel_educacional_abandono");
            Map(nameof(PainelEducacionalAbandono.AnoLetivo), "ano_letivo");
            Map(nameof(PainelEducacionalAbandono.CodigoDre), "codigo_dre");
            Map(nameof(PainelEducacionalAbandono.CodigoUe), "codigo_ue");
            Map(nameof(PainelEducacionalAbandono.Ano), "ano");
            Map(nameof(PainelEducacionalAbandono.QuantidadeDesistencias), "quantidade_desistencias");
            Map(nameof(PainelEducacionalAbandono.Modalidade), "modalidade");
            Map(nameof(PainelEducacionalAbandono.Turma), "turma");
            Map(nameof(PainelEducacionalAbandono.CriadoEm), "criado_em");
        }
    }
}