using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoReflexoFrequenciaBuscaAtivaAlunoMap : BaseMap<ConsolidacaoReflexoFrequenciaBuscaAtivaAluno>
    {
        public ConsolidacaoReflexoFrequenciaBuscaAtivaAlunoMap()
        {
            ToTable("consolidacao_reflexo_frequencia_busca_ativa");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.TurmaCodigo), "turma_id");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.UeCodigo), "ue_id");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.AnoLetivo), "ano_letivo");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.Modalidade), "modalidade_codigo");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.AlunoCodigo), "aluno_codigo");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.AlunoNome), "aluno_nome");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.DataBuscaAtiva), "data_acao");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.Mes), "mes");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.PercFrequenciaAntesAcao), "percentual_frequencia_anterior_acao");
            Map(nameof(ConsolidacaoReflexoFrequenciaBuscaAtivaAluno.PercFrequenciaAposAcao), "percentual_frequencia_atual");
        }
    }
}