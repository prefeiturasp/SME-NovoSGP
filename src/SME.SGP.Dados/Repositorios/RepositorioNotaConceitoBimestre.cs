using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotaConceitoBimestre : RepositorioBase<NotaConceitoBimestre>, IRepositorioNotaConceitoBimestre
    {
        public RepositorioNotaConceitoBimestre(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<NotaConceitoBimestre>> ObterPorFechamentoTurma(long fechamentoId)
        {
            var query = "select * from nota_conceito_bimestre where fechamento_turma_disciplina_id = @fechamentoId";

            return await database.Conexao.QueryAsync<NotaConceitoBimestre>(query, new { fechamentoId });
        }
    }
}