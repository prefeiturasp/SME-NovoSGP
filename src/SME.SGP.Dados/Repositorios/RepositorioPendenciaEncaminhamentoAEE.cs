using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaEncaminhamentoAEE : RepositorioBase<PendenciaEncaminhamentoAEE>, IRepositorioPendenciaEncaminhamentoAEE
    {
        public RepositorioPendenciaEncaminhamentoAEE(ISgpContext database) : base(database)
        {
        }

        public async Task<PendenciaEncaminhamentoAEE> ObterPorEncaminhamentoAEEId(long encaminhamentoAEEId)
        {
            var sql = @"select * from pendencia_encaminhamento_aee where encaminhamento_aee_id = @encaminhamentoAEEId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PendenciaEncaminhamentoAEE>(sql, new { encaminhamentoAEEId });
        }

        public async Task<IEnumerable<PendenciaEncaminhamentoAEE>> ObterPendenciasPorEncaminhamentoAEEId(long encaminhamentoAEEId)
        {
            var sql = @"select * from pendencia_encaminhamento_aee where encaminhamento_aee_id = @encaminhamentoAEEId";

            return await database.Conexao.QueryAsync<PendenciaEncaminhamentoAEE>(sql, new { encaminhamentoAEEId });
        }

        public async Task Excluir(long pendenciaId)
        {
            await database.Conexao.ExecuteScalarAsync(@"delete from pendencia_encaminhamento_aee 
                                                    where pendencia_id = @pendenciaId", new { pendenciaId });
        }

        public async Task<PendenciaEncaminhamentoAEE> ObterPorEncaminhamentoAEEIdEUsuarioId(long encaminhamentoAEEId)
        {
            var sql = @"select * from pendencia_encaminhamento_aee pea
                        where pea.encaminhamento_aee_id = @encaminhamentoAEEId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PendenciaEncaminhamentoAEE>(sql, new { encaminhamentoAEEId});
        }
    }
}