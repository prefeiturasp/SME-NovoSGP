using System;
using System.Threading.Tasks;
using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestral : IRepositorioRelatorioSemestral
    {
        private readonly ISgpContext database;

        public RepositorioRelatorioSemestral(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<RelatorioSemestral> ObterPorIdAsync(long id)
        {
            var query = "select * from repositorio_semestral where id = @id";

            return await database.Conexao.QueryFirstOrDefaultAsync<RelatorioSemestral>(query, new { id });
        }

        public Task<RelatorioSemestral> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre)
        {
            throw new System.NotImplementedException();
        }

        public async Task SalvarAsync(RelatorioSemestral relatorioSemestral)
        {
            if (relatorioSemestral.Id > 0)
                await database.Conexao.UpdateAsync(relatorioSemestral);
            else
                relatorioSemestral.Id = (long)await database.Conexao.InsertAsync(relatorioSemestral);
        }

    }
}