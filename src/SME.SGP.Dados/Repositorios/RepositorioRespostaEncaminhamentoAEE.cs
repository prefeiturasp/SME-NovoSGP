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
					set excluido = true 
					where arquivo_id = @arquivoId 
                ";

			return (
				database
				.Conexao
				.Execute(sql, new { arquivoId })
				) > 0;
		}
	}
}
