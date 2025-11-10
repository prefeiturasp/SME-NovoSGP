using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalAprovacaoUe : IRepositorioPainelEducacionalAprovacaoUe
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalAprovacaoUe(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_aprovacao_ue 
                    (codigo_dre, codigo_ue, turma, modalidade, total_promocoes, total_retencoes_ausencias, total_retencoes_notas, ano_letivo, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.Turma, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.Modalidade, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.TotalPromocoes, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.TotalRetencoesAusencias, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.TotalRetencoesNotas, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.AnoLetivo, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.CriadoEm, NpgsqlDbType.TimestampTz);
            }

            await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao()
        {
            var sql = @"TRUNCATE painel_educacional_consolidacao_aprovacao_ue";

            await database.ExecuteAsync(sql);
        }
        public async Task<PaginacaoResultadoDto<PainelEducacionalConsolidacaoAprovacaoUe>> ObterAprovacao(
           int anoLetivo,
           string codigoUe,
           int numeroPagina,
           int numeroRegistros)
        {
            var query = MontarQueryConsulta(codigoUe);
            var queryPaginacao = MontarQueryConsulta(codigoUe, true);

            var paginacao = new Paginacao(numeroPagina, numeroRegistros);

            var parametrosTotalPaginas = MontarParametros(anoLetivo, codigoUe, null);
            var parametrosQuery = MontarParametros(anoLetivo, codigoUe, paginacao);

            var totalRegistros = await database.Conexao.QueryFirstOrDefaultAsync<int>(queryPaginacao, parametrosTotalPaginas);
            var totalPaginas = totalRegistros == 0
                ? 0
                : (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros);

            var registros = await database.Conexao.QueryAsync<PainelEducacionalConsolidacaoAprovacaoUe>(query, parametrosQuery);

            return ObterResultado(registros, totalPaginas, totalRegistros);
        }

        private static string MontarQueryConsulta(string codigoUe, bool ehContador = false)
        {
            var sb = new StringBuilder();

            if (ehContador)
            {
                sb.AppendLine("SELECT COUNT(*) FROM painel_educacional_consolidacao_aprovacao_ue WHERE ano_letivo = @anoLetivo");
                if (!string.IsNullOrEmpty(codigoUe)) sb.AppendLine(" AND codigo_ue = @codigoUe");
            }
            else
            {
                sb.AppendLine(@"SELECT 
                                    codigo_dre AS CodigoDre,
                                    codigo_ue AS CodigoUe,
                                    turma AS Turma,
                                    modalidade AS Modalidade,
                                    total_promocoes AS TotalPromocoes,
                                    total_retencoes_ausencias AS TotalRetencoesAusencias,
                                    total_retencoes_notas AS TotalRetencoesNotas,
                                    ano_letivo AS AnoLetivo,
                                    criado_em AS CriadoEm
                                FROM painel_educacional_consolidacao_aprovacao_ue
                                WHERE ano_letivo = @anoLetivo");

                if (!string.IsNullOrEmpty(codigoUe))
                    sb.AppendLine(" AND codigo_ue = @codigoUe");

                sb.AppendLine(" ORDER BY turma");
                sb.AppendLine(" OFFSET @offset ROWS FETCH NEXT @numeroRegistros ROWS ONLY");
            }

            return sb.ToString();
        }

        private static object MontarParametros(int anoLetivo, string codigoUe, Paginacao paginacao)
        {
            return paginacao != null
                ? new
                {
                    anoLetivo,
                    codigoUe,
                    offset = paginacao.QuantidadeRegistrosIgnorados,
                    numeroRegistros = paginacao.QuantidadeRegistros
                }
                : new { anoLetivo, codigoUe };
        }

        private static PaginacaoResultadoDto<PainelEducacionalConsolidacaoAprovacaoUe> ObterResultado(
            IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> registros,
            int totalPaginas,
            int totalRegistros)
        {
            return new PaginacaoResultadoDto<PainelEducacionalConsolidacaoAprovacaoUe>
            {
                Items = registros,
                TotalPaginas = totalPaginas,
                TotalRegistros = totalRegistros
            };
        }
    }
}
