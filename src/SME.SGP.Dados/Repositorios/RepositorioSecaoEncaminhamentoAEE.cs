using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoEncaminhamentoAEE : RepositorioBase<SecaoEncaminhamentoAEE>, IRepositorioSecaoEncaminhamentoAEE
    {
        public RepositorioSecaoEncaminhamentoAEE(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecaoEncaminhamentoDtoPorEtapa(List<int> etapas, long encaminhamentoAeeId = 0)
        {
            var query = @"SELECT sea.id
                                , sea.nome
                                , sea.questionario_id as questionarioId
                                , eas.concluido
                                , sea.etapa
                         FROM secao_encaminhamento_aee sea
                        left join encaminhamento_aee_secao eas on eas.encaminhamento_aee_id = @encaminhamentoAeeId and eas.secao_encaminhamento_id = sea.id
                         WHERE not sea.excluido 
                           AND sea.etapa = ANY(@etapas)
                         ORDER BY sea.etapa, sea.ordem ";

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(query, new { etapas, encaminhamentoAeeId });
        }

        public async Task<IEnumerable<SecaoEncaminhamentoAEE>> ObterSecoesEncaminhamentoPorEtapa(List<int> etapas, long encaminhamentoAeeId = 0)
        {
            var query = @"SELECT sea.*
                                , eas.*
                         FROM secao_encaminhamento_aee sea
                        left join encaminhamento_aee_secao eas on eas.encaminhamento_aee_id = @encaminhamentoAeeId and eas.secao_encaminhamento_id = sea.id
                         WHERE not sea.excluido 
                           AND sea.etapa = ANY(@etapas)
                         ORDER BY sea.etapa, sea.ordem ";

            return await database.Conexao.QueryAsync<SecaoEncaminhamentoAEE, EncaminhamentoAEESecao, SecaoEncaminhamentoAEE>(query
                , (secaoEncaminhamento, encaminhamentoSecao) =>
                {
                    secaoEncaminhamento.EncaminhamentoAEESecao = encaminhamentoSecao;
                    return secaoEncaminhamento;
                }
                , new { etapas, encaminhamentoAeeId });
        }
    }
}
