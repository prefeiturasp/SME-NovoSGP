using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoEncaminhamentoAEE : RepositorioBase<QuestaoEncaminhamentoAEE>, IRepositorioQuestaoEncaminhamentoAEE
    {
        public RepositorioQuestaoEncaminhamentoAEE(ISgpContext repositorio) : base(repositorio)
        {
        }

        public async Task<IEnumerable<Questao>> ObterListaPorQuestionario(long questionarioId)
        {
            var query = @"select q.*, op.*
                          from questao q 
                          left join opcao_resposta op on op.questao_id = q.id
                         where q.questionario_id = @questionarioId ";

            var lookup = new Dictionary<long, Questao>();
            await database.Conexao.QueryAsync<Questao, OpcaoResposta, Questao>(query,
                (questao, opcaoResposta) =>
                {
                    var q = new Questao();
                    if (!lookup.TryGetValue(questao.Id, out q))
                    {
                        q = questao;
                        lookup.Add(q.Id, q);
                    }

                    if (opcaoResposta != null)
                    {
                        q.OpcoesRespostas.Add(opcaoResposta);
                    }

                    return q;
                }, new { questionarioId });

            return lookup.Values;
        }

        public async Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoAEESecaoId)
        {
            var query = "select id from questao_encaminhamento_aee qea where encaminhamento_aee_secao_id = @encaminhamentoAEESecaoId";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoAEESecaoId });
        }

        public async Task<IEnumerable<RespostaQuestaoEncaminhamentoAEEDto>> ObterRespostasEncaminhamento(long encaminhamentoId)
        {
            var query = @"select rea.Id
                            , qea.questao_id as QuestaoId
	                        , rea.resposta_id as RespostaId
	                        , rea.texto 
	                        , a.*
                          from encaminhamento_aee_secao eas 
                         inner join questao_encaminhamento_aee qea on qea.encaminhamento_aee_secao_id = eas.id
                         inner join resposta_encaminhamento_aee rea on rea.questao_encaminhamento_id = qea.id
                          left join arquivo a on a.id = rea.arquivo_id 
                         where eas.encaminhamento_aee_id = @encaminhamentoId ";

            return await database.Conexao.QueryAsync<RespostaQuestaoEncaminhamentoAEEDto, Arquivo, RespostaQuestaoEncaminhamentoAEEDto>(query,
                (resposta, arquivo) =>
                {
                    resposta.Arquivo = arquivo;
                    return resposta;
                }, new { encaminhamentoId });
        }
    }
}
