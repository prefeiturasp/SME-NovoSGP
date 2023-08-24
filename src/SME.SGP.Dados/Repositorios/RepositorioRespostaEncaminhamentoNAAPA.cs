using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
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
                await database
                .Conexao
                .ExecuteAsync(sql, new { arquivoId })
                ) > 0;
        }

        public async Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoEncaminhamentoNAAPAId)
        {
            var query = "select arquivo_id from encaminhamento_naapa_resposta where questao_encaminhamento_id = @questaoEncaminhamentoNAAPAId and arquivo_id is not null";

            return await database.Conexao.QueryAsync<long>(query, new { questaoEncaminhamentoNAAPAId });
        }

    }
}
