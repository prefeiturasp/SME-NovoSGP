using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConceito : RepositorioBase<Conceito>, IRepositorioConceito
    {
        public RepositorioConceito(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<Conceito>> ObterPorIds(long[] ids)
        {
            const string sql = @"select id, valor, descricao, aprovado, ativo, inicio_vigencia, fim_vigencia,
                    criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
                    from conceito_valores where id = any(@ids)";

            return await database.Conexao.QueryAsync<Conceito>(sql, new { ids });
        }
    }
}