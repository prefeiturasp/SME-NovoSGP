using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe>> ObterAprovacao(
     int anoLetivo,
     string codigoUe,
     string modalidade,
     int numeroPagina,
     int numeroRegistros)
        {
            var sql = @"SELECT *
                FROM painel_educacional_consolidacao_aprovacao_ue
                WHERE ano_letivo = @anoLetivo";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND codigo_ue = @codigoUe";

            if (!string.IsNullOrWhiteSpace(modalidade))
                sql += " AND LOWER(modalidade) = LOWER(@modalidade)";

            sql += @" ORDER BY codigo_ue
              OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

            var offset = (numeroPagina - 1) * numeroRegistros;
            var limit = numeroRegistros;

            return await database.QueryAsync<PainelEducacionalConsolidacaoAprovacaoUe>(
                sql,
                new
                {
                    anoLetivo,
                    codigoUe,
                    modalidade,
                    offset,
                    limit
                });
        }

    }
}
