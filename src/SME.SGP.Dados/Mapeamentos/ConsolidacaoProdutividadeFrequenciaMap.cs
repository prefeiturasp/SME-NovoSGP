using Dapper.FluentMap.Dommel.Mapping;
using MongoDB.Bson;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoProdutividadeFrequenciaMap : BaseMap<ConsolidacaoProdutividadeFrequencia>
    {
        public ConsolidacaoProdutividadeFrequenciaMap()
        {
            ToTable("consolidacao_produtividade_frequencia");
            Map(c => c.CodigoTurma).ToColumn("turma_id");
            Map(c => c.DescricaoTurma).ToColumn("turma_desc");
            Map(c => c.CodigoUe).ToColumn("ue_id");
            Map(c => c.DescricaoUe).ToColumn("ue_desc");
            Map(c => c.CodigoDre).ToColumn("dre_id");
            Map(c => c.DescricaoDre).ToColumn("dre_desc");
            Map(c => c.NomeProfessor).ToColumn("professor_nm");
            Map(c => c.RfProfessor).ToColumn("professor_rf");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.Modalidade).ToColumn("modalidade_codigo");
            Map(c => c.DataAula).ToColumn("data_aula");
            Map(c => c.DataRegistroFrequencia).ToColumn("data_reg_freq");
            Map(c => c.DiferenciaDiasDataAulaRegistroFrequencia).ToColumn("dif_data_aula_reg_freq");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.CodigoComponenteCurricular).ToColumn("componente_curricular_id");
            Map(c => c.NomeComponenteCurricular).ToColumn("componente_curricular_nm");
        }
    }
}