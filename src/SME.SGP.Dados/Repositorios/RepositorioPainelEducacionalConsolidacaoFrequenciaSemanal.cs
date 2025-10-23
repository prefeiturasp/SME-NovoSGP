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
    public class RepositorioPainelEducacionalConsolidacaoFrequenciaSemanal : IRepositorioPainelEducacionalConsolidacaoFrequenciaSemanal
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalConsolidacaoFrequenciaSemanal(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoFrequenciaSemanal> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_frequencia_semanal 
                    (codigo_dre, codigo_ue, ano_letivo, total_estudantes, total_presentes, percentual_frequencia, data_aula, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.AnoLetivo, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.TotalEstudantes, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.TotalPresentes, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.PercentualFrequencia, NpgsqlDbType.Numeric);
                await writer.WriteAsync(item.DataAula, NpgsqlDbType.Timestamp);
                await writer.WriteAsync(item.CriadoEm, NpgsqlDbType.TimestampTz);
            }

            await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao()
        {
            var sql = @"TRUNCATE painel_educacional_consolidacao_frequencia_semanal";

            await database.ExecuteAsync(sql);
        }
    }
}
