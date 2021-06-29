using Dapper;
using Dommel;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class RepositorioConsolidacaoAcompanhamentoAprendizagemAluno : IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoAcompanhamentoAprendizagemAluno(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> Inserir(ConsolidacaoAcompanhamentoAprendizagemAluno consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task Limpar()
        {
            var query = @" delete from consolidacao_acompanhamento_aprendizagem_aluno ";

            await database.Conexao.ExecuteScalarAsync(query, new { });
        }
    }
}
