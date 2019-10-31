using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEvento : RepositorioBase<Evento>, IRepositorioEvento
    {
        public RepositorioEvento(ISgpContext conexao) : base(conexao)
        {
        }

        public bool ExisteEventoNaMesmaDataECalendario(DateTime dataInicio, long tipoCalendarioId)
        {
            var query = "select 1 from evento where data_inicio = @dataInicio and tipo_calendario = @tipoCalendarioId;";
            return database.Conexao.QueryFirstOrDefault<bool>(query, new { dataInicio, tipoCalendarioId });
        }

        public async Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosMesesFiltroDto calendarioEventosMesesFiltro)
        {
            var query = new StringBuilder();
            query.AppendLine("SELECT a.mes,");
            query.AppendLine("Count(*) eventos");
            query.AppendLine("FROM(SELECT a.*,");
            query.AppendLine("Rank()");
            query.AppendLine("over(");
            query.AppendLine("PARTITION BY a.id, a.mes");
            query.AppendLine("ORDER BY campo) rank_id");
            query.AppendLine("FROM(SELECT id,");
            query.AppendLine("Extract(month FROM data_inicio) mes,");
            query.AppendLine("1 campo");
            query.AppendLine("FROM   evento");
            query.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and dre_id = @DreId");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and ue_id = @UeId");

            if (calendarioEventosMesesFiltro.EhEventoSme)
                query.AppendLine("and ue_id is null and dre_id is null");

            query.AppendLine("UNION ALL");
            query.AppendLine("SELECT id,");
            query.AppendLine("Extract(month FROM data_fim) mes_evento,");
            query.AppendLine("2 campo");
            query.AppendLine("FROM   evento");
            query.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and dre_id = @DreId");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and ue_id = @UeId");

            if (calendarioEventosMesesFiltro.EhEventoSme)
                query.AppendLine("and ue_id is null and dre_id is null");

            query.AppendLine(") a) a");
            query.AppendLine("WHERE  a.rank_id = 1");
            query.AppendLine("GROUP BY a.mes");

            return await database.Conexao.QueryAsync<CalendarioEventosMesesDto>(query.ToString(), new
            {
                IdTipoCalendario = calendarioEventosMesesFiltro.IdTipoCalendario,
                DreId = calendarioEventosMesesFiltro.DreId,
                UeId = calendarioEventosMesesFiltro.UeId
            });
        }
    }
}