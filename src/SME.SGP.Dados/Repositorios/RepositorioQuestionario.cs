using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestionario : RepositorioBase<Questionario>, IRepositorioQuestionario
    {
        public RepositorioQuestionario(ISgpContext database) : base(database)
        {
        }

        public async Task<long> ObterQuestionarioIdPorTipo(int tipoQuestionario)
        {
            var query = @"select id 
                          from questionario q
                         where q.tipo = @tipoQuestionario";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { tipoQuestionario });
        }

        public async Task<IEnumerable<Questao>> ObterQuestoesPorQuestionarioId(long questionarioId)
        {
            var query = @"select q.*, op.*, oqc.*
                          from questao q 
                          left join opcao_resposta op on op.questao_id = q.id
                          left join opcao_questao_complementar oqc on oqc.opcao_resposta_id = op.id
                         where q.questionario_id = @questionarioId 
                        order by q.id, op.id";

            var lookup = new Dictionary<long, Questao>();
            await database.Conexao.QueryAsync<Questao, OpcaoResposta, OpcaoQuestaoComplementar, Questao>(query,
                (questao, opcaoResposta, OpcaoQuestaoComplementar) =>
                {
                    var q = new Questao();
                    if (!lookup.TryGetValue(questao.Id, out q))
                    {
                        q = questao;
                        lookup.Add(q.Id, q);
                    }

                    var entidadeOpcaoResposta = q.OpcoesRespostas.FirstOrDefault(a => a.Id == opcaoResposta.Id);
                    if (entidadeOpcaoResposta == null && opcaoResposta != null)
                    {
                        q.OpcoesRespostas.Add(opcaoResposta);
                        entidadeOpcaoResposta = opcaoResposta;
                    }

                    if (OpcaoQuestaoComplementar != null)
                    {
                        entidadeOpcaoResposta.QuestoesComplementares.Add(OpcaoQuestaoComplementar);
                    }

                    return q;
                }, new { questionarioId });

            return lookup.Values;
        }
    }
}
