using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoFrequenciaAlunoMensalMap : SimpleMap<ConsolidacaoFrequenciaAlunoMensal>
    {
        public ConsolidacaoFrequenciaAlunoMensalMap()
        {
            ToTable("consolidacao_frequencia_aluno_mensal");
            Map(nameof(ConsolidacaoFrequenciaAlunoMensal.TurmaId), "turma_id");
            Map(nameof(ConsolidacaoFrequenciaAlunoMensal.AlunoCodigo), "aluno_codigo");
            Map(nameof(ConsolidacaoFrequenciaAlunoMensal.Mes), "mes");
            Map(nameof(ConsolidacaoFrequenciaAlunoMensal.Percentual), "percentual");
            Map(nameof(ConsolidacaoFrequenciaAlunoMensal.QuantidadeAulas), "quantidade_aulas");
            Map(nameof(ConsolidacaoFrequenciaAlunoMensal.QuantidadeAusencias), "quantidade_ausencias");
            Map(nameof(ConsolidacaoFrequenciaAlunoMensal.QuantidadeCompensacoes), "quantidade_compensacoes");
        }
    }
}