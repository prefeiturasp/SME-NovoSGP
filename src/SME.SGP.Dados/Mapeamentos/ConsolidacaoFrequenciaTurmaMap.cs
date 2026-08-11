using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoFrequenciaTurmaMap : SimpleMap<ConsolidacaoFrequenciaTurma>
    {
        public ConsolidacaoFrequenciaTurmaMap()
        {
            ToTable("consolidacao_frequencia_turma");
            Map(nameof(ConsolidacaoFrequenciaTurma.TurmaId), "turma_id");
            Map(nameof(ConsolidacaoFrequenciaTurma.QuantidadeAcimaMinimoFrequencia), "quantidade_acima_minimo_frequencia");
            Map(nameof(ConsolidacaoFrequenciaTurma.QuantidadeAbaixoMinimoFrequencia), "quantidade_abaixo_minimo_frequencia");
            Map(nameof(ConsolidacaoFrequenciaTurma.TipoConsolidacao), "tipo_consolidacao");
            Map(nameof(ConsolidacaoFrequenciaTurma.PeriodoInicio), "periodo_inicio");
            Map(nameof(ConsolidacaoFrequenciaTurma.PeriodoFim), "periodo_fim");
            Map(nameof(ConsolidacaoFrequenciaTurma.TotalAulas), "total_aulas");
            Map(nameof(ConsolidacaoFrequenciaTurma.TotalFrequencias), "total_frequencias");
        }
    }
}