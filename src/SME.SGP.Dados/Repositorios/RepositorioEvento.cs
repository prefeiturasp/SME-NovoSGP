using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
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

        public async Task<PaginacaoResultadoDto<Evento>> Listar(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim, Paginacao paginacao)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            MontaQueryFrom(query);
            MontaQueryFiltro(tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, query);
            query.AppendLine("order by e.id");
            if (paginacao != null && paginacao.QuantidadeRegistros > 0)
                MontaQueryPaginacao(query);
            else
                paginacao = new Paginacao(1, 0);

            if (!string.IsNullOrEmpty(nomeEvento))
            {
                nomeEvento = $"%{nomeEvento}%";
            }

            var retornoPaginado = new PaginacaoResultadoDto<Evento>();

            retornoPaginado.Items = await database.Conexao.QueryAsync<Evento, EventoTipo, Evento>(query.ToString(), (evento, tipoEvento) =>
            {
                evento.AdicionarTipoEvento(tipoEvento);
                return evento;
            }, new
            {
                tipoCalendarioId,
                tipoEventoId,
                nomeEvento,
                dataInicio,
                dataFim,
                ignorar = paginacao.QuantidadeRegistrosIgnorados,
                quantidadeBuscar = paginacao.QuantidadeRegistros
            },
            splitOn: "EventoId,TipoEventoId");

            var queryCount = new StringBuilder("select count(e.*)");
            MontaQueryFrom(queryCount);
            MontaQueryFiltro(tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, queryCount);
            retornoPaginado.TotalRegistros = await database.Conexao.QueryFirstOrDefaultAsync<int>(queryCount.ToString(), new
            {
                tipoCalendarioId,
                tipoEventoId,
                nomeEvento,
                dataInicio,
                dataFim
            });

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        public async Task<IEnumerable<EventosPorDiaRetornoQueryDto>> ObterQuantidadeDeEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes)
        {
            var query = new StringBuilder();

            query.AppendLine("select a.dia, a.tipoevento from(select id, extract(day from e.data_inicio) as dia,");
            query.AppendLine("case");
            query.AppendLine("when e.dre_id is not null then 'DRE'");
            query.AppendLine("when e.ue_id is not null then 'UE'");
            query.AppendLine("when e.ue_id is null and e.dre_id is null then 'SME'");
            query.AppendLine("end as TipoEvento");
            query.AppendLine("from evento e");
            query.AppendLine("where excluido = false");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and dre_id = @DreId");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and ue_id = @UeId");

            if (calendarioEventosMesesFiltro.EhEventoSme)
                query.AppendLine("and ue_id is null and dre_id is null");

            if (mes > 0)
                query.AppendLine("and extract(month from e.data_inicio) = @mes");

            query.AppendLine("group by id, dia, tipoevento");
            query.AppendLine("union distinct");
            query.AppendLine("select id, extract(day from e.data_fim) as dia,   ");
            query.AppendLine("case");
            query.AppendLine("when e.dre_id is not null then 'DRE'");
            query.AppendLine("when e.ue_id is not null then 'UE'");
            query.AppendLine("when e.ue_id is null and e.dre_id is null then 'SME'");
            query.AppendLine("end as TipoEvento");
            query.AppendLine("from evento e");
            query.AppendLine("where excluido = false");
            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and dre_id = @DreId");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and ue_id = @UeId");

            if (calendarioEventosMesesFiltro.EhEventoSme)
                query.AppendLine("and ue_id is null and dre_id is null");

            if (mes > 0)
                query.AppendLine("and extract(month from e.data_fim) = @mes");

            query.AppendLine("group by id, dia, tipoevento) a");
            query.AppendLine("order by a.dia");

            return await database.Conexao.QueryAsync<EventosPorDiaRetornoQueryDto>(query.ToString(), new
            {
                IdTipoCalendario = calendarioEventosMesesFiltro.IdTipoCalendario,
                DreId = calendarioEventosMesesFiltro.DreId,
                UeId = calendarioEventosMesesFiltro.UeId,
                mes
            });
        }

        public async Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosFiltroDto calendarioEventosMesesFiltro)
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
            query.AppendLine("where excluido = false");

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

        private static void MontaQueryCabecalho(StringBuilder query)
        {
            query.AppendLine("select");
            query.AppendLine("e.id as EventoId,");
            query.AppendLine("e.id,");
            query.AppendLine("e.nome,");
            query.AppendLine("e.descricao,");
            query.AppendLine("e.data_inicio,");
            query.AppendLine("e.data_fim,");
            query.AppendLine("e.dre_id,");
            query.AppendLine("e.letivo,");
            query.AppendLine("e.feriado_id,");
            query.AppendLine("e.tipo_calendario_id,");
            query.AppendLine("e.tipo_evento_id,");
            query.AppendLine("e.ue_id,");
            query.AppendLine("e.criado_em,");
            query.AppendLine("e.criado_por,");
            query.AppendLine("e.alterado_em,");
            query.AppendLine("e.alterado_por,");
            query.AppendLine("e.criado_rf,");
            query.AppendLine("e.alterado_rf,");
            query.AppendLine("et.id as TipoEventoId,");
            query.AppendLine("et.id,");
            query.AppendLine("et.ativo,");
            query.AppendLine("et.tipo_data,");
            query.AppendLine("et.descricao,");
            query.AppendLine("et.excluido");
        }

        private static void MontaQueryFiltro(long? tipoCalendarioId, long? tipoEventoId, DateTime? dataInicio, DateTime? dataFim, string nomeEvento, StringBuilder query)
        {
            query.AppendLine("where");
            query.AppendLine("e.excluido = false");
            query.AppendLine("and et.ativo = true");
            query.AppendLine("and et.excluido = false");

            if (tipoCalendarioId.HasValue)
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            if (tipoEventoId.HasValue)
                query.AppendLine("and e.tipo_evento_id = @tipoEventoId");

            if (dataInicio.HasValue && dataFim.HasValue)
            {
                query.AppendLine("and e.data_inicio >= @dataInicio");
                query.AppendLine("and (e.data_fim is null OR e.data_fim <= @dataFim)");
            }
            if (!string.IsNullOrWhiteSpace(nomeEvento))
            {
                query.AppendLine("and lower(f_unaccent(e.nome)) LIKE @nomeEvento");
            }
        }

        private static void MontaQueryFrom(StringBuilder query)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
        }

        private static void MontaQueryPaginacao(StringBuilder query)
        {
            query.AppendLine(" OFFSET @ignorar ROWS FETCH NEXT @quantidadeBuscar ROWS ONLY");
        }
    }
}