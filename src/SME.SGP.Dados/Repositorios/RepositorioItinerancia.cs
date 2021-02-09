using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItinerancia : RepositorioBase<Itinerancia>, IRepositorioItinerancia
    {
        public RepositorioItinerancia(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<ItineranciaObjetivosBaseDto>> ObterObjetivosBase()
        {
            var query = @"select id,
	                             nome,
	                             tem_descricao as TemDescricao,
	                             permite_varias_ues as PermiteVariasUes
                            from itinerancia_objetivo_base iob  
                           where not excluido 
                           order by ordem  ";

            return await database.Conexao.QueryAsync<ItineranciaObjetivosBaseDto>(query);
        }

        public async Task<IEnumerable<ItineranciaAlunoQuestaoDto>> ObterQuestoesItineranciaAluno(long id)
        {
            var query = @"select iaq.id,
                                 iaq.questao_id as QuestaoId,       
                                 iaq.resposta as Resposta
                            from itinerancia_aluno_questao iaq 
                           where iaq.itinerancia_aluno_id = @id
                             and not iaq.excluido ";

            return await database.Conexao.QueryAsync<ItineranciaAlunoQuestaoDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaQuestaoBaseDto>> ObterItineranciaQuestaoBase()
        {
            var query = @"select q.id, q.nome, q.ordem, q1.tipo 
                            from questao q
                           inner join questionario q1 on q1.id = q.questionario_id 
                           where q1.tipo in (2,3)
                             and not q.excluido";

            return await database.Conexao.QueryAsync<ItineranciaQuestaoBaseDto>(query);
        }
    }
}
