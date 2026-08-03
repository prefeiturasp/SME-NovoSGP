using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunoMap : SimpleMap<ConsolidacaoAcompanhamentoAprendizagemAluno>
    {
        public ConsolidacaoAcompanhamentoAprendizagemAlunoMap()
        {
            ToTable("consolidacao_acompanhamento_aprendizagem_aluno");
            Map(nameof(ConsolidacaoAcompanhamentoAprendizagemAluno.TurmaId),"turma_id");
            Map(nameof(ConsolidacaoAcompanhamentoAprendizagemAluno.QuantidadeComAcompanhamento),"quantidade_com_acompanhamento");
            Map(nameof(ConsolidacaoAcompanhamentoAprendizagemAluno.QuantidadeSemAcompanhamento),"quantidade_sem_acompanhamento");
            Map(nameof(ConsolidacaoAcompanhamentoAprendizagemAluno.Semestre),"semestre");
        }
    }
}
