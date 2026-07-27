using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoDashBoardFrequenciaMap : SimpleEntityMap<ConsolidacaoDashBoardFrequencia>
    {
        public ConsolidacaoDashBoardFrequenciaMap()
        {
            ToTable("consolidado_dashboard_frequencia");
            Map(nameof(ConsolidacaoDashBoardFrequencia.TurmaId),"turma_id");
            Map(nameof(ConsolidacaoDashBoardFrequencia.TurmaNome),"turma_nome");
            Map(nameof(ConsolidacaoDashBoardFrequencia.TurmaAno),"turma_ano");
            Map(nameof(ConsolidacaoDashBoardFrequencia.DataAula),"data_aula");
            Map(nameof(ConsolidacaoDashBoardFrequencia.DataInicio),"data_inicio_semana");
            Map(nameof(ConsolidacaoDashBoardFrequencia.DataFim),"data_fim_semana");
            Map(nameof(ConsolidacaoDashBoardFrequencia.ModalidadeCodigo),"modalidade_codigo");
            Map(nameof(ConsolidacaoDashBoardFrequencia.AnoLetivo),"ano_letivo");
            Map(nameof(ConsolidacaoDashBoardFrequencia.DreId),"dre_id");
            Map(nameof(ConsolidacaoDashBoardFrequencia.DreCodigo),"dre_codigo");
            Map(nameof(ConsolidacaoDashBoardFrequencia.UeId),"ue_id");
            Map(nameof(ConsolidacaoDashBoardFrequencia.DreAbreviacao),"dre_abreviacao");
            Map(nameof(ConsolidacaoDashBoardFrequencia.QuantidadePresencas),"quantidade_presencas");
            Map(nameof(ConsolidacaoDashBoardFrequencia.QuantidadeAusentes),"quantidade_ausencias");
            Map(nameof(ConsolidacaoDashBoardFrequencia.QuantidadeRemotos),"quantidade_remotos");
            Map(nameof(ConsolidacaoDashBoardFrequencia.CriadoEm),"criado_em");
            Map(nameof(ConsolidacaoDashBoardFrequencia.semestre),"semestre");
            Map(nameof(ConsolidacaoDashBoardFrequencia.Mes),"mes");
            Map(nameof(ConsolidacaoDashBoardFrequencia.Tipo),"tipo");
            Map(nameof(ConsolidacaoDashBoardFrequencia.TotalAulas),"total_aulas");
            Map(nameof(ConsolidacaoDashBoardFrequencia.TotalFrequencias),"total_frequencias");
        }
    }
}
