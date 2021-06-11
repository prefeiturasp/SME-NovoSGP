using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoEncaminhamentoAEE : RepositorioBase<QuestaoEncaminhamentoAEE>, IRepositorioQuestaoEncaminhamentoAEE
    {
        public RepositorioQuestaoEncaminhamentoAEE(ISgpContext repositorio) : base(repositorio)
        {
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
                         where not eas.excluido 
                           and not qea.excluido 
                           and not rea.excluido 
                           and eas.encaminhamento_aee_id = @encaminhamentoId ";

            return await database.Conexao.QueryAsync<RespostaQuestaoEncaminhamentoAEEDto, Arquivo, RespostaQuestaoEncaminhamentoAEEDto>(query,
                (resposta, arquivo) =>
                {
                    resposta.Arquivo = arquivo;
                    return resposta;
                }, new { encaminhamentoId });
        }
    }
}
