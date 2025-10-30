using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoDistorcaoIdade : IRepositorioPainelEducacionalConsolidacaoDistorcaoIdade
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalConsolidacaoDistorcaoIdade(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoDistorcaoIdade> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                    COPY painel_educacional_consolidacao_distorcao_serie_idade
                        (ano_letivo, codigo_dre, codigo_ue, modalidade, ano, quantidade_alunos, criado_em)
                    FROM STDIN (FORMAT BINARY)
                ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.AnoLetivo, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.Modalidade, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.Ano, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.QuantidadeAlunos, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.CriadoEm, NpgsqlDbType.TimestampTz);
            }

            await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao(int ano)
        {
            var sql = @"DELETE FROM painel_educacional_consolidacao_distorcao_serie_idade WHERE ano_letivo >= @ano;
                        SELECT setval(
                            pg_get_serial_sequence('painel_educacional_consolidacao_distorcao_serie_idade', 'id'),
                            (SELECT MAX(id) FROM painel_educacional_consolidacao_distorcao_serie_idade)
                        );
                        ";

            await database.ExecuteAsync(sql, new { ano });
        }

        public async Task<int?> ObterUltimoAnoConsolidadoAsync()
        {
            var sql = @"SELECT MAX(ano_letivo) FROM painel_educacional_consolidacao_distorcao_serie_idade";
            return await database.QueryFirstOrDefaultAsync<int?>(sql);
        }
    }
}
