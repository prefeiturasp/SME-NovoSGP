using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestionario : RepositorioBase<Questionario>, IRepositorioQuestionario
    {
        public RepositorioQuestionario(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<long> ObterQuestionarioIdPorTipo(int tipoQuestionario)
        {
            var query = @"select id 
                          from questionario q
                         where q.tipo = @tipoQuestionario";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { tipoQuestionario });
        }

        public Task<IEnumerable<Questao>> ObterQuestoesPorNomesComponentes(string[] nomesComponentes, TipoQuestionario tipoQuestionario)
        {
            var filtro = @"qto.tipo = @tipoQuestionario
                           AND q.nome_componente = ANY(@nomesComponentes)";

            return ObterQuestoes(filtro, new { nomesComponentes, tipoQuestionario = (int)tipoQuestionario });
        }

        public Task<IEnumerable<Questao>> ObterQuestoesPorQuestionarioId(long questionarioId)
        {
            return ObterQuestoes("q.questionario_id = @questionarioId", new { questionarioId });
        }

        private async Task<IEnumerable<Questao>> ObterQuestoes(string condicao, object parametro)
        {
            var query = @$"select q.*, op.*, oqc.*
                          from questao q 
                          inner join questionario qto on qto.id = q.questionario_id
                          left join opcao_resposta op on op.questao_id = q.id
                          left join opcao_questao_complementar oqc on oqc.opcao_resposta_id = op.id
                         where {condicao}
                            and not q.excluido 
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
                    if (entidadeOpcaoResposta.EhNulo() && opcaoResposta.NaoEhNulo())
                    {
                        q.OpcoesRespostas.Add(opcaoResposta);
                        entidadeOpcaoResposta = opcaoResposta;
                    }

                    if (OpcaoQuestaoComplementar.NaoEhNulo())
                    {
                        entidadeOpcaoResposta.QuestoesComplementares.Add(OpcaoQuestaoComplementar);
                    }

                    return q;
                }, parametro);

            return lookup.Values;
        }
    }
}
