using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
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
            var query = @"select q.id, q.nome, q.ordem, q1.tipo, q.obrigatorio 
                            from questao q
                           inner join questionario q1 on q1.id = q.questionario_id 
                           where q1.tipo in (2,3)
                             and not q.excluido";

            return await database.Conexao.QueryAsync<ItineranciaQuestaoBaseDto>(query);
        }

        public async Task<ItineranciaDto> ObterItineranciaPorId(long id)
        {
            var query = @"select id, 
                                 data_visita as DataVisita, 
                                 data_retorno_verificacao as DataRetornoVerificacao 
                            from itinerancia i 
                           where id = @id
                             and not excluido";

            return await database.Conexao.QueryFirstOrDefaultAsync<ItineranciaDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaAlunoDto>> ObterItineranciaAlunoPorId(long id)
        {
            var query = @" select id,
 		                          codigo_aluno as CodigoAluno
                             from itinerancia_aluno ia 
                            where itinerancia_id = @id
                              and not excluido ";

            return await database.Conexao.QueryAsync<ItineranciaAlunoDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaObjetivoDto>> ObterObjetivosItineranciaPorId(long id)
        {
            var query = @"select iob.id,
                                 iob.nome,
                                 iob.tem_descricao,
                                 iob.permite_varias_ues,
                                 io.descricao 
                            from itinerancia_objetivo_base iob 
                           inner join itinerancia_objetivo io on io.itinerancia_base_id = iob.id 
                           where io.itinerancia_id = @id
                             and not io.excluido 
                           order by iob.ordem";

            return await database.Conexao.QueryAsync<ItineranciaObjetivoDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaQuestaoDto>> ObterQuestoesItineranciaPorId(long id)
        {
            var query = @"select iq.id,
                                 iq.questao_id as QuestaoId, 
                                 q.nome as Descricao,
                                 iq.resposta,
                                 iq.itinerancia_id as ItineranciaId,
                                 q.obrigatorio
                            from questao q
                           inner join questionario q1 on q1.id = q.questionario_id
                           inner join itinerancia_questao iq on iq.questao_id = q.id 
                           where iq.itinerancia_id = @Id
                             and q1.tipo in (2)
                             and not q.excluido
                           order by q.ordem";

            return await database.Conexao.QueryAsync<ItineranciaQuestaoDto>(query, new { id });
        }

        public async Task<IEnumerable<ItineranciaUeDto>> ObterUesItineranciaPorId(long id)
        {
            var query = @"select iu.id,
       		                     iu.ue_id as UeId,
       		                     ue.nome as Descricao
                            from itinerancia_ue iu 
                           inner join ue on ue.id = iu.ue_id 
                           where iu.itinerancia_id = @Id
                             and not iu.excluido";

            return await database.Conexao.QueryAsync<ItineranciaUeDto>(query, new { id });
        }
        public async Task<Itinerancia> ObterEntidadeCompleta(long id)
        {
            var query = @"select i.*, ia.*, iaq.*, iq.* , io.*, iob.*, iu.*
                            from itinerancia i 
                           left join itinerancia_aluno ia on ia.itinerancia_id = i.id
                           left join itinerancia_aluno_questao iaq on iaq.itinerancia_aluno_id = ia.id    
                           left join itinerancia_questao iq on iq.itinerancia_id = i.id 
                           left join itinerancia_objetivo io on io.itinerancia_id = i.id   
                           left join itinerancia_objetivo_base iob on iob.id = io.itinerancia_base_id
                           left join itinerancia_ue iu on iu.itinerancia_id = i.id                           
                           where i.id = @id
                             and not i.excluido";

            var lookup = new Dictionary<long, Itinerancia>();

             database.Conexao.Query<Itinerancia, ItineranciaAluno, ItineranciaAlunoQuestao, ItineranciaQuestao, ItineranciaObjetivo, ItineranciaObjetivoBase, ItineranciaUe, Itinerancia>(query,
                 (registroItinerancia, itineranciaAluno, itineranciaAlunoquestao, itineranciaQuestao, itineranciaObjetivo, itineranciaObjetivoBase, itineranciaUe) =>
                 {
                     Itinerancia itinerancia;
                     if (!lookup.TryGetValue(registroItinerancia.Id, out itinerancia))
                     {
                         itinerancia = registroItinerancia;
                         lookup.Add(registroItinerancia.Id, itinerancia);
                     }
                     if(itineranciaAluno != null)
                        itinerancia.AdicionarAluno(itineranciaAluno);

                     if (itineranciaAlunoquestao != null)
                         itinerancia.AdicionarQuestaoAluno(itineranciaAluno.Id, itineranciaAlunoquestao);

                     if (itineranciaQuestao != null)
                         itinerancia.AdicionarQuestao(itineranciaQuestao);

                     if (itineranciaObjetivo != null)
                         itinerancia.AdicionarObjetivo(itineranciaObjetivo);

                     if (itineranciaObjetivoBase != null)
                         itinerancia.AdicionarObjetivoBase(itineranciaObjetivoBase);

                     if (itineranciaUe != null)
                         itinerancia.AdicionarUe(itineranciaUe);

                     return itinerancia;
                 }, param: new { id });

            return lookup.Values.FirstOrDefault();            
        }
    }
}
