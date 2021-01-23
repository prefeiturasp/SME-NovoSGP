using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaEncaminhamentoAEE : RepositorioBase<RespostaEncaminhamentoAEE>, IRepositorioRespostaEncaminhamentoAEE
    {
        public RepositorioRespostaEncaminhamentoAEE(ISgpContext database) : base(database)
        {
        }

        public async Task<bool> RemoverPorArquivoId(long arquivoId)
        {
			await Task.CompletedTask;

			var sql =
				$@"
					update resposta_encaminhamento_aee 
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
        
        public async Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoEncaminhamentoAEEId)
        {
            var query = "select arquivo_id from resposta_encaminhamento_aee where questao_encaminhamento_id = @questaoEncaminhamentoAEEId and arquivo_id is not null";

            return await database.Conexao.QueryAsync<long>(query, new { questaoEncaminhamentoAEEId });
        }

        public async Task<IEnumerable<RespostaEncaminhamentoAEE>> ObterPorQuestaoEncaminhamentoId(long questaoEncaminhamentoAEEId)
        {
            var query = "select * from resposta_encaminhamento_aee where not excluido and questao_encaminhamento_id = @questaoEncaminhamentoAEEId";

            return await database.Conexao.QueryAsync<RespostaEncaminhamentoAEE>(query, new { questaoEncaminhamentoAEEId });
        }
    }
}
