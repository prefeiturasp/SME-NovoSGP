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
    public class RepositorioPainelEducacionalConsolidacaoEducacaoIntegral : IRepositorioPainelEducacionalConsolidacaoEducacaoIntegral
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalConsolidacaoEducacaoIntegral(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task<int?> ObterUltimoAnoConsolidado()
        {
            const string query = @"select max(ano_letivo) from painel_educacional_consolidacao_educacao_integral";
            return await database.QueryFirstOrDefaultAsync<int?>(query);
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoEducacaoIntegral> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_educacao_integral 
                    (ano_letivo, codigo_dre, codigo_ue,	modalidade_turma, ano, quantidade_alunos_integral, quantidade_alunos_parcial, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.AnoLetivo, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.ModalidadeTurma, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.Ano, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.QuantidadeAlunosIntegral, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.QuantidadeAlunosParcial, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.CriadoEm, NpgsqlDbType.TimestampTz);
            }

            await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao(int ano)
        {
            var sql = @"DELETE FROM painel_educacional_consolidacao_educacao_integral WHERE ano_letivo = @ano;
                        SELECT setval(
                            pg_get_serial_sequence('painel_educacional_consolidacao_educacao_integral', 'id'),
                            (SELECT MAX(id) FROM painel_educacional_consolidacao_educacao_integral)
                        );
                        ";

            await database.ExecuteAsync(sql, new { ano });
        }
    }
}
