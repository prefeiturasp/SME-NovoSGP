using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoEncaminhamentoNAAPA : RepositorioBase<QuestaoEncaminhamentoNAAPA>, IRepositorioQuestaoEncaminhamentoNAAPA
    {
        public RepositorioQuestaoEncaminhamentoNAAPA(ISgpContext repositorio, IServicoAuditoria servicoAuditoria) : base(repositorio, servicoAuditoria)
        {

        }

        public async Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoNAAPASecaoId)
        {
            var query = "select id from encaminhamento_naapa_questao qea where encaminhamento_naapa_secao_id = @encaminhamentoNAAPASecaoId";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoNAAPASecaoId });
        }

        public async Task<IEnumerable<RespostaQuestaoEncaminhamentoNAAPADto>> ObterRespostasEncaminhamento(long encaminhamentoId)
        {
            var query = @"select ren.Id
                            , qen.questao_id as QuestaoId
	                        , ren.resposta_id as RespostaId
	                        , ren.texto 
	                        , a.*
                          from encaminhamento_naapa_secao ens 
                         inner join encaminhamento_naapa_questao qen on qen.encaminhamento_naapa_secao_id = ens.id
                         inner join encaminhamento_naapa_resposta ren on ren.questao_encaminhamento_id = qen.id
                          left join arquivo a on a.id = ren.arquivo_id 
                         where not ens.excluido 
                           and not qen.excluido 
                           and not ren.excluido 
                           and ens.encaminhamento_naapa_id = @encaminhamentoId ";

            return await database.Conexao.QueryAsync<RespostaQuestaoEncaminhamentoNAAPADto, Arquivo, RespostaQuestaoEncaminhamentoNAAPADto>(query,
                (resposta, arquivo) =>
                {
                    resposta.Arquivo = arquivo;
                    return resposta;
                }, new { encaminhamentoId });
        }
    }
}
