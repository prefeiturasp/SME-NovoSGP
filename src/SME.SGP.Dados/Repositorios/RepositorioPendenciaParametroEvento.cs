using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaParametroEvento : RepositorioBase<PendenciaParametroEvento>, IRepositorioPendenciaParametroEvento
    {
        public RepositorioPendenciaParametroEvento(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PendenciaParametroEventoDto>> ObterPendenciasEventoPorPendenciaId(long pendenciaId)
        {
            var query = @"select ps.descricao, ps.valor 
                          from pendencia_parametro_evento pe 
                        inner join parametros_sistema ps on ps.id = pe.parametro_sistema_id
                         where pe.pendencia_id = @pendenciaId";

            return await database.Conexao.QueryAsync<PendenciaParametroEventoDto>(query, new { pendenciaId });
        }
    }
}
