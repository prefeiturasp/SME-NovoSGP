using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalAprovacao : IRepositorioPainelEducacionalAprovacao
    {
        private readonly ISgpContext database;
        private readonly IConfiguration configuration;
        public RepositorioPainelEducacionalAprovacao(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalAprovacao> registros)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_registro_frequencia_agrupamento_global
                    (codigo_dre, codigo_ue, modalidade, total_aulas, total_ausencias, percentual_frequencia, total_alunos, ano_letivo, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var r in registros)
            {
                writer.StartRow();
                writer.Write(r.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.Modalidade, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.TotalAulas, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.TotalAusencias, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.PercentualFrequencia, NpgsqlTypes.NpgsqlDbType.Numeric);
                writer.Write(r.TotalAlunos, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            }

            await writer.CompleteAsync();
        }

        public async Task ExcluirAprovacao(int anoLetivo)
        {
            const string comando = @"delete from public.painel_educacional_registro_frequencia_agrupamento_global where ano_letivo = @anoLetivo";

            await database.Conexao.ExecuteAsync(comando, new {anoLetivo});
        }

        public async Task<IEnumerable<PainelEducacionalAprovacao>> ObterAprovacao(int anoLetivo, string codigoDre, string codigoUe)
        {
            var sql = $@"SELECT * FROM painel_educacional_registro_frequencia_agrupamento_global
                        WHERE 1=1";

            if (anoLetivo > 0)
                sql += " AND ano_letivo = @anoLetivo";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                sql += " AND codigo_dre = @codigoDre";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND codigo_ue = @codigoUe";

            return await database.QueryAsync<PainelEducacionalAprovacao>(sql, new { anoLetivo, codigoDre, codigoUe });
        }
    }
}
