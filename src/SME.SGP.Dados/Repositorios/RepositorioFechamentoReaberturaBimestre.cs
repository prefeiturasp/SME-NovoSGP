using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoReaberturaBimestre : RepositorioBase<FechamentoReaberturaBimestre>, IRepositorioFechamentoReaberturaBimestre
    {
        public RepositorioFechamentoReaberturaBimestre(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<FechamentoReaberturaBimestre>> ObterPorFechamentoReaberturaIdAsync(long fechamentoReaberturaId)
        {
            var query = @"select * from fechamento_reabertura_bimestre where fechamento_reabertura_id = @fechamentoReaberturaId";

            return await database.Conexao.QueryAsync<FechamentoReaberturaBimestre>(query, new { fechamentoReaberturaId });
        }
    }
}
