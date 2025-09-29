using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal : IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal
    {
        private readonly ISgpContext database;
        private readonly IConfiguration configuration;
        public RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal(ISgpContext database, IConfiguration configuration)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.configuration = configuration;
        }

        public async Task ExcluirFrequenciaMensal(int anoLetivo)
        {
            const string comando = @"delete from public.painel_educacional_registro_frequencia_agrupamento_mensal where ano_letivo = @anoLetivo";

            await database.Conexao.ExecuteAsync(comando, new { anoLetivo });
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>> ObterFrequenciaMensal(int anoLetivo, string codigoDre, string codigoUe)
        {
            var sql = $@"SELECT * FROM painel_educacional_registro_frequencia_agrupamento_mensal
                        WHERE 1=1";

            if (anoLetivo > 0)
                sql += " AND ano_letivo = @anoLetivo";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                sql += " AND codigo_dre = @codigoDre";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND codigo_ue = @codigoUe";

            sql += " ORDER BY modalidade";

            return await database.QueryAsync<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>(sql, new { anoLetivo, codigoDre, codigoUe });
        }

        public async Task<bool> SalvarFrequenciaMensal(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal> consolidacoes)
        {
            var sql = @"
                        INSERT INTO painel_educacional_registro_frequencia_agrupamento_mensal
                            (
                                codigo_dre,
                                codigo_ue,
                                modalidade,
                                ano_letivo,
                                mes,
                                total_aulas,
                                total_faltas,
                                percentual_frequencia,
                                criado_em
                            )
                        VALUES (
                                @CodigoDre,
                                @CodigoUe,
                                @Modalidade,
                                @AnoLetivo,
                                @Mes,
                                @TotalAulas,
                                @TotalFaltas,
                                @PercentualFrequencia,
                                @CriadoEm
                            );";

            int rowsAffected = await database.ExecuteAsync(sql, consolidacoes);

            return rowsAffected > 0;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal> registros)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            // Inicia o COPY em modo binário
            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_registro_frequencia_agrupamento_mensal
                    (codigo_dre, codigo_ue, modalidade, ano_letivo, mes, total_aulas, total_faltas, percentual_frequencia, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var r in registros)
            {
                writer.StartRow();
                writer.Write(r.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.Modalidade, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.Mes, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.TotalAulas, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.TotalFaltas, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.PercentualFrequencia, NpgsqlTypes.NpgsqlDbType.Numeric);
                writer.Write(r.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            }

            await writer.CompleteAsync();
        }
    }
}
