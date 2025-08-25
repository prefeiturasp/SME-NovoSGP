using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoGlobalMap : BaseMap<PainelEducacionalRegistroFrequenciaAgrupamentoGlobal>
    {
        public PainelEducacionalRegistroFrequenciaAgrupamentoGlobalMap()
        {
            ToTable("painel_educacional_registro_frequencia_agrupamento_global");
            Map(c => c.Modalidade).ToColumn("modalidade");
            Map(c => c.TotalAulas).ToColumn("total_aulas");
            Map(c => c.TotalAusencias).ToColumn("total_ausencias");
            Map(c => c.TotalCompensacoes).ToColumn("total_compensacoes");
            Map(c => c.PercentualFrequencia).ToColumn("percentual_frequencia");
            Map(c => c.TotalAlunos).ToColumn("total_alunos");
        }
    }
}
