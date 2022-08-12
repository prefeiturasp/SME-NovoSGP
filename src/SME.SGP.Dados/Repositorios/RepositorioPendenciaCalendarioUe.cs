using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaCalendarioUe : RepositorioBase<PendenciaCalendarioUe>, IRepositorioPendenciaCalendarioUe
    {
        public RepositorioPendenciaCalendarioUe(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PendenciaCalendarioUe>> ObterPendenciasPorCalendarioUe(long tipoCalendarioId, long ueId, TipoPendencia tipoPendencia)
        {
            var query = @"select pc.* 
                        from pendencia_calendario_ue pc
                       inner join pendencia p on p.id = pc.pendencia_id
                        where not p.excluido 
                          and pc.tipo_calendario_id = @tipoCalendarioId 
                          and pc.ue_id = @ueId
                          and p.tipo = @tipoPendencia";

            return await database.Conexao.QueryAsync<PendenciaCalendarioUe>(query, new { tipoCalendarioId, ueId, tipoPendencia });
        }
    }
}
