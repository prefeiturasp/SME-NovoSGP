using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoRegistroAcaoBuscaAtiva : RepositorioBase<QuestaoRegistroAcaoBuscaAtiva>, IRepositorioQuestaoRegistroAcaoBuscaAtiva
    {
        public RepositorioQuestaoRegistroAcaoBuscaAtiva(ISgpContext repositorio, IServicoAuditoria servicoAuditoria) : base(repositorio, servicoAuditoria)
        {
        }

        public Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long registroAcaoSecaoId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<RespostaQuestaoRegistroAcaoBuscaAtivaDto>> ObterRespostasRegistroAcao(long registroAcaoId)
        {
            var query = @$"select rabar.Id
                            , rabaq.questao_id as QuestaoId
                            , rabar.resposta_id as RespostaId
                            , rabar.texto 
                            , a.*
                          from registro_acao_busca_ativa_secao rabas  
                         inner join registro_acao_busca_ativa_questao rabaq on rabaq.registro_acao_busca_ativa_secao_id = rabas.id
                         inner join registro_acao_busca_ativa_resposta rabar on rabar.questao_registro_acao_id = rabaq.id
                          left join arquivo a on a.id = rabar.arquivo_id 
                         where not rabas.excluido 
                           and not rabaq.excluido 
                           and not rabar.excluido 
                           and rabas.registro_acao_busca_ativa_id = @registroAcaoId";

            return await database.Conexao.QueryAsync<RespostaQuestaoRegistroAcaoBuscaAtivaDto, Arquivo, RespostaQuestaoRegistroAcaoBuscaAtivaDto>(query,
                (resposta, arquivo) =>
                {
                    resposta.Arquivo = arquivo;
                    return resposta;
                }, new { registroAcaoId });
        }

    }
}
