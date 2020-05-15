using System;
using System.Threading.Tasks;
using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestralTurmaPAP : IRepositorioRelatorioSemestralTurmaPAP
    {
        private readonly ISgpContext database;

        public RepositorioRelatorioSemestralTurmaPAP(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<RelatorioSemestralTurmaPAP> ObterPorIdAsync(long id)
        {
            var query = "select * from relatorio_semestral_turma_pap where id = @id";

            return await database.Conexao.QueryFirstOrDefaultAsync<RelatorioSemestralTurmaPAP>(query, new { id });
        }

        public Task<RelatorioSemestralTurmaPAP> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre)
        {
            throw new System.NotImplementedException();
        }

        public async Task SalvarAsync(RelatorioSemestralTurmaPAP relatorioSemestral)
        {
            if (relatorioSemestral.Id > 0)
                await database.Conexao.UpdateAsync(relatorioSemestral);
            else
                relatorioSemestral.Id = (long)await database.Conexao.InsertAsync(relatorioSemestral);
        }

    }
}