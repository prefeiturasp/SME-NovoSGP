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
        private const int ETAPA_1 = 1;
        private const string QUESTAO_PRIORIDADE = "Prioridade";

        public RepositorioQuestaoEncaminhamentoNAAPA(ISgpContext repositorio, IServicoAuditoria servicoAuditoria) : base(repositorio, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<PrioridadeEncaminhamentoNAAPADto>> ObterPrioridadeEncaminhamento()
        {
            var query = @"select opre.id, opre.nome
                            from secao_encaminhamento_naapa sen
                            inner join questionario qti on qti.id = sen.questionario_id
                            inner join questao qta on qta.questionario_id = qti.id
                            inner join opcao_resposta opre on opre.questao_id = qta.id
                            where sen.etapa = @etapa and qta.nome = @questaoPrioridade
                            order by opre.ordem";

            return await database.Conexao.QueryAsync<PrioridadeEncaminhamentoNAAPADto>(query, new { etapa = ETAPA_1, questaoPrioridade = QUESTAO_PRIORIDADE });
        }

        public async Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoNAAPASecaoId)
        {
            var query = "select id from encaminhamento_naapa_questao qea where encaminhamento_naapa_secao_id = @encaminhamentoNAAPASecaoId";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoNAAPASecaoId });
        }

        public async Task<IEnumerable<RespostaQuestaoEncaminhamentoNAAPADto>> ObterRespostasEncaminhamento(long encaminhamentoId)
        {
            return await ObterRespostasEncaminhamento(encaminhamentoId, "ens.encaminhamento_naapa_id = @encaminhamentoId");
        }

        public async Task<IEnumerable<RespostaQuestaoEncaminhamentoNAAPADto>> ObterRespostasItinerarioEncaminhamento(long encaminhamentoSecaoId)
        {
            return await ObterRespostasEncaminhamento(encaminhamentoSecaoId, "ens.id = @encaminhamentoId");
        }

        private async Task<IEnumerable<RespostaQuestaoEncaminhamentoNAAPADto>> ObterRespostasEncaminhamento(long encaminhamentoId, string condicao)
        {
            var query = @$"select ren.Id
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
                           and {condicao}";

            return await database.Conexao.QueryAsync<RespostaQuestaoEncaminhamentoNAAPADto, Arquivo, RespostaQuestaoEncaminhamentoNAAPADto>(query,
                (resposta, arquivo) =>
                {
                    resposta.Arquivo = arquivo;
                    return resposta;
                }, new { encaminhamentoId });
        }
    }
}
