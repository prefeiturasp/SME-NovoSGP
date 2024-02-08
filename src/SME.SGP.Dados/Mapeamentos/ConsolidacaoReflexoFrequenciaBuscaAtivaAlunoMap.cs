using Dapper.FluentMap.Dommel.Mapping;
using MongoDB.Bson;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoReflexoFrequenciaBuscaAtivaAlunoMap : BaseMap<ConsolidacaoReflexoFrequenciaBuscaAtivaAluno>
    {
        public ConsolidacaoReflexoFrequenciaBuscaAtivaAlunoMap()
        {
            ToTable("consolidacao_reflexo_frequencia_buscaativa");
            Map(c => c.TurmaCodigo).ToColumn("turma_id");
            Map(c => c.DreCodigo).ToColumn("ue_id");
            Map(c => c.UeCodigo).ToColumn("dre_id");
            Map(c => c.AnoTurma).ToColumn("ano_turma");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Modalidade).ToColumn("modalidade_codigo");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.AlunoNome).ToColumn("aluno_nome");
            Map(c => c.DataBuscaAtiva).ToColumn("data_acao");           
            Map(c => c.Mes).ToColumn("mes");
            Map(c => c.PercFrequenciaAntesAcao).ToColumn("percentual_frequencia_anterior_acao");
            Map(c => c.PercFrequenciaAposAcao).ToColumn("percentual_frequencia_atual");
        }
    }
}