using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaParametroEvento : RepositorioBase<PendenciaParametroEvento>, IRepositorioPendenciaParametroEvento
    {
        public RepositorioPendenciaParametroEvento(ISgpContext database) : base(database)
        {
        }

        public async Task<PendenciaParametroEvento> ObterPendenciaEventoPorCalendarioUeParametro(long tipoCalendarioId, long ueId, long parametroId)
        {
            var query = @"select ppe.*, pc.*
                          from pendencia_parametro_evento ppe 
                         inner join pendencia_calendario_ue pc on pc.id = ppe.pendencia_calendario_ue_id
                         inner join pendencia p on p.id = pc.pendencia_id
                         where not p.excluido
                           and pc.tipo_calendario_id = @tipoCalendarioId
                           and pc.ue_id = @ueId
                           and ppe.parametro_sistema_id = @parametroId";

            return (await database.Conexao.QueryAsync<PendenciaParametroEvento, PendenciaCalendarioUe, PendenciaParametroEvento>(query,
                (pendenciaParametroEvento, pendenciaCalendarioUe) =>
                {
                    pendenciaParametroEvento.PendenciaCalendarioUe = pendenciaCalendarioUe;
                    return pendenciaParametroEvento;
                },
                new { tipoCalendarioId, ueId, parametroId })).FirstOrDefault();
        }

        public async Task<PendenciaParametroEvento> ObterPendenciaEventoPorPendenciaEParametroId(long pendenciaId, long parametroId)
        {
            var query = @"select pe.*
                          from pendencia_parametro_evento pe 
                        inner join pendencia_calendario_ue pc on pc.id = pe.pendencia_calendario_ue_id
                        inner join parametros_sistema ps on ps.id = pe.parametro_sistema_id
                         where pc.pendencia_id = @pendenciaId
                           and pe.parametro_sistema_id = @parametroId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PendenciaParametroEvento>(query, new { pendenciaId, parametroId });
        }

        public async Task<IEnumerable<PendenciaParametroEvento>> ObterPendenciasEventoPorPendenciaCalendarioUe(long pendenciaCalendarioUeId)
        {
            var query = @"select pe.*
                          from pendencia_parametro_evento pe 
                         where pe.pendencia_calendario_ue_id = @pendenciaCalendarioUeId";

            return await database.Conexao.QueryAsync<PendenciaParametroEvento>(query, new { pendenciaCalendarioUeId });
        }

        public async Task<IEnumerable<PendenciaParametroEventoDto>> ObterPendenciasEventoPorPendenciaId(long pendenciaId)
        {
            var query = @"select ps.id as ParametroSistemaId, ps.descricao, pe.quantidade_eventos as valor
                          from pendencia_parametro_evento pe 
                        inner join pendencia_calendario_ue pc on pc.id = pe.pendencia_calendario_ue_id
                        inner join parametros_sistema ps on ps.id = pe.parametro_sistema_id
                         where pc.pendencia_id = @pendenciaId";

            return await database.Conexao.QueryAsync<PendenciaParametroEventoDto>(query, new { pendenciaId });
        }
    }
}
