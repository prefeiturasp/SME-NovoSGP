using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaEncaminhamentoNAAPA : RepositorioBase<RespostaEncaminhamentoNAAPA>, IRepositorioRespostaEncaminhamentoNAAPA
    {
        public RepositorioRespostaEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
        
        public async Task<IEnumerable<RespostaEncaminhamentoNAAPA>> ObterPorQuestaoEncaminhamentoId(long questaoEncaminhamentoNAAPAId)
        {
            var query = "select * from encaminhamento_naapa_resposta where not excluido and questao_encaminhamento_id = @questaoEncaminhamentoNAAPAId";

            return await database.Conexao.QueryAsync<RespostaEncaminhamentoNAAPA>(query, new { questaoEncaminhamentoNAAPAId });
        }

        public async Task<bool> RemoverPorArquivoId(long arquivoId)
        {
            await Task.CompletedTask;

            var sql =
                $@"
					update encaminhamento_naapa_resposta 
                        set excluido = true,
                            arquivo_id = null
					where arquivo_id = @arquivoId 
                ";

            return (
                database
                .Conexao
                .Execute(sql, new { arquivoId })
                ) > 0;
        }

        public async Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoEncaminhamentoNAAPAId)
        {
            var query = "select arquivo_id from encaminhamento_naapa_resposta where questao_encaminhamento_id = @questaoEncaminhamentoNAAPAId and arquivo_id is not null";

            return await database.Conexao.QueryAsync<long>(query, new { questaoEncaminhamentoNAAPAId });
        }

        public async Task<IEnumerable<RespostaEncaminhamentoNAAPA>> ObterRespostaEnderecoResidencialPorEncaminhamentoId(long encaminhamentoAEEId)
        {
            var query = @"select enr.*, enq.*, q.* from encaminhamento_naapa_resposta enr
                            inner join encaminhamento_naapa_questao enq on enq.id = enr.questao_encaminhamento_id 
                            inner join encaminhamento_naapa_secao ens on ens.id = enq.encaminhamento_naapa_secao_id 
                            inner join secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id 
                            inner join questao q on q.id = enq.questao_id 
                            where not enr.excluido and not enq.excluido and not ens.excluido 
                                and sen.nome_componente = 'INFORMACOES_ESTUDANTE' 
                                and q.nome_componente = 'ENDERECO_RESIDENCIAL'
	                            and ens.encaminhamento_naapa_id  = @encaminhamentoNAAPAId";

            return await database.Conexao.QueryAsync<RespostaEncaminhamentoNAAPA, QuestaoEncaminhamentoNAAPA, Questao, RespostaEncaminhamentoNAAPA>(query, 
                                        (respostaNAAPA, questaoNAAPA, questao) =>
                                        {
                                            questaoNAAPA.Questao = questao;
                                            respostaNAAPA.QuestaoEncaminhamento = questaoNAAPA;
                                            return respostaNAAPA;

                                        },  
                                        new { encaminhamentoAEEId });
        }
    }
}
