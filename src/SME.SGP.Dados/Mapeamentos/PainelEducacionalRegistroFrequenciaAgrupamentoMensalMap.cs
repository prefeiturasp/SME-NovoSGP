using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoMensalMap : BaseMap<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>
    {
        public PainelEducacionalRegistroFrequenciaAgrupamentoMensalMap()
        {
            ToTable("painel_educacional_registro_frequencia_agrupamento_mensal");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Mes).ToColumn("mes");
            Map(c => c.TotalAulas).ToColumn("total_aulas");
            Map(c => c.TotalFaltas).ToColumn("total_faltas");
            Map(c => c.PercentualFrequencia).ToColumn("percentual_frequencia");
        }
    }
}
