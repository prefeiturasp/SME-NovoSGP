using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoAcompanhamentoAprendizagemAlunoMap : DommelEntityMap<ConsolidacaoAcompanhamentoAprendizagemAluno>
    {
        public ConsolidacaoAcompanhamentoAprendizagemAlunoMap()
        {
            ToTable("consolidacao_acompanhamento_aprendizagem_aluno");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.QuantidadeComAcompanhamento).ToColumn("quantidade_com_acompanhamento");
            Map(c => c.QuantidadeSemAcompanhamento).ToColumn("quantidade_sem_acompanhamento");
            Map(c => c.Semestre).ToColumn("semestre");
        }
    }
}
