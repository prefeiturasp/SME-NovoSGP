using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoDashBoardFrequenciaMap : DommelEntityMap<ConsolidacaoDashBoardFrequencia>
    {
        public ConsolidacaoDashBoardFrequenciaMap()
        {
            ToTable("consolidado_dashboard_frequencia");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.TurmaNome).ToColumn("turma_nome");
            Map(c => c.TurmaAno).ToColumn("turma_ano");
            Map(c => c.DataAula).ToColumn("data_aula");
            Map(c => c.DataInicio).ToColumn("data_inicio");
            Map(c => c.DataFim).ToColumn("data_fim");
            Map(c => c.Mes).ToColumn("mes");
            Map(c => c.ModalidadeCodigo).ToColumn("modalidade_codigo");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.DreAbreviacao).ToColumn("dre_abreviacao");
            Map(c => c.QuantidadePresencas).ToColumn("quantidadepresencas");
            Map(c => c.QuantidadeAusentes).ToColumn("quantidadeausencias");
            Map(c => c.QuantidadeRemotos).ToColumn("quantidaderemotos");
            Map(c => c.CriadoEm).ToColumn("criado_em");
        }
    }
}
