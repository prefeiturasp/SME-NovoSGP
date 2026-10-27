using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalConsolidacaoFrequenciaSemanal
        : IRepositorioPainelEducacionalConsolidacaoFrequenciaSemanal
    {
        private readonly IConfiguration _configuration;
        private readonly ISgpContext _database;

        public RepositorioPainelEducacionalConsolidacaoFrequenciaSemanal(
            IConfiguration configuration,
            ISgpContext database)
        {
            _configuration = configuration;
            _database = database;
        }

        public async Task BulkInsertAsync(
            IEnumerable<PainelEducacionalConsolidacaoFrequenciaSemanal> indicadores)
        {
            await using var conn = new NpgsqlConnection(
                _configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

#pragma warning disable S6966
            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_frequencia_semanal
                    (codigo_dre, codigo_ue, ano_letivo, total_estudantes,
                     total_presentes, percentual_frequencia, data_aula, criado_em)
                FROM STDIN (FORMAT BINARY)");
#pragma warning restore S6966

            foreach (var item in indicadores)
                await WriteFrequenciaRowAsync(writer, item);

            await writer.CompleteAsync();
        }

        private static async Task WriteFrequenciaRowAsync(
            NpgsqlBinaryImporter writer,
            PainelEducacionalConsolidacaoFrequenciaSemanal item)
        {
            await writer.StartRowAsync();
            await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
            await writer.WriteAsync(item.CodigoUe, NpgsqlDbType.Varchar);
            await writer.WriteAsync(item.AnoLetivo, NpgsqlDbType.Integer);
            await writer.WriteAsync(item.TotalEstudantes, NpgsqlDbType.Integer);
            await writer.WriteAsync(item.TotalPresentes, NpgsqlDbType.Integer);
            await writer.WriteAsync(item.PercentualFrequencia, NpgsqlDbType.Numeric);
            await writer.WriteAsync(
                DateTimeExtension.EnsureUnspecified(item.DataAula), NpgsqlDbType.Timestamp);
            await writer.WriteAsync(
                DateTimeExtension.EnsureUnspecified(item.CriadoEm), NpgsqlDbType.Timestamp);
        }

        public async Task LimparConsolidacao()
        {
            const string sql = @"TRUNCATE painel_educacional_consolidacao_frequencia_semanal";
            await _database.ExecuteAsync(sql);
        }
    }
}