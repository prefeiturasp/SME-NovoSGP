using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoProdutividadeFrequenciaMap : BaseEntityMap<ConsolidacaoProdutividadeFrequencia>
    {
        public ConsolidacaoProdutividadeFrequenciaMap()
        {
            ToTable("consolidacao_produtividade_frequencia");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.CodigoTurma), "turma_id");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.DescricaoTurma), "turma_desc");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.CodigoUe), "ue_id");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.DescricaoUe), "ue_desc");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.CodigoDre), "dre_id");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.DescricaoDre), "dre_desc");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.NomeProfessor), "professor_nm");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.RfProfessor), "professor_rf");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.Bimestre), "bimestre");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.Modalidade), "modalidade_codigo");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.DataAula), "data_aula");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.DataRegistroFrequencia), "data_reg_freq");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.DiferenciaDiasDataAulaRegistroFrequencia), "dif_data_aula_reg_freq");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.AnoLetivo), "ano_letivo");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.CodigoComponenteCurricular), "componente_curricular_id");
            Map(nameof(ConsolidacaoProdutividadeFrequencia.NomeComponenteCurricular), "componente_curricular_nm");
        }
    }
}