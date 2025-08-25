using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoEscolaMap : BaseMap<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>
    {
        public PainelEducacionalRegistroFrequenciaAgrupamentoEscolaMap()
        {
            ToTable("painel_educacional_registro_frequencia_agrupamento_escola");
            Map(c => c.CodigoUe).ToColumn("codigo_ue");
            Map(c => c.UE).ToColumn("ue");
            Map(c => c.TotalAulas).ToColumn("total_aulas");
            Map(c => c.TotalAusencias).ToColumn("total_ausencias");
            Map(c => c.TotalCompensacoes).ToColumn("total_compensacoes");
            Map(c => c.PercentualFrequencia).ToColumn("percentual_frequencia");
            Map(c => c.TotalAlunos).ToColumn("total_alunos");
        }
    }
}
