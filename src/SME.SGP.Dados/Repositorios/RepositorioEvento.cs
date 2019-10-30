using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
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

            query.AppendLine("select count(e.*) as Total");

            MontaQueryFrom(query);
            MontaQueryFiltro(tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, query);
            query.Append(";");

            query.AppendLine("select");
            query.AppendLine("e.id as EventoId,");
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
            query.AppendLine("et.ativo,");
            query.AppendLine("et.descricao,");
            query.AppendLine("et.excluido");

            MontaQueryFrom(query);
            MontaQueryFiltro(tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, query);
            query.Append(" OFFSET @ignorar ROWS FETCH NEXT @quantidadeBuscar ROWS ONLY");

            query.Append(";");

            if (!string.IsNullOrEmpty(nomeEvento))
            {
                nomeEvento = $"%{nomeEvento}%";
            }

            var retornoPaginado = new PaginacaoResultadoDto<Evento>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(),
            new
            {
                tipoCalendarioId,
                tipoEventoId,
                nomeEvento,
                ignorar = paginacao.QuantidadeRegistrosIgnorados,
                quantidadeBuscar = paginacao.QuantidadeRegistros
            }))
            {
                retornoPaginado.TotalRegistros = multi.ReadFirstOrDefault<int>();
                retornoPaginado.Items = multi.Read<Evento>();
            }

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
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
                query.AppendLine("and e.data_inicio = @dataInicio");
                query.AppendLine("and e.data_fim = @dataFim");
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
    }
}