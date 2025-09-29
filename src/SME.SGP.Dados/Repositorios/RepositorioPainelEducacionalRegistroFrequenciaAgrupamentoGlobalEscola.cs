using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola : IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola
    {
        private readonly ISgpContext database;
        private readonly IConfiguration configuration;
        public RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola(ISgpContext database, IConfiguration configuration)
        {
            this.database = database;
            this.configuration = configuration;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola> registros)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            // Inicia o COPY em modo binário
            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_registro_frequencia_agrupamento_escola
                    (total_aulas, total_ausencias, percentual_frequencia, total_alunos,
                     codigo_dre, codigo_ue, ue, dre, mes, ano_letivo, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var r in registros)
            {
                writer.StartRow();
                writer.Write(r.TotalAulas, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.TotalAusencias, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.PercentualFrequencia, NpgsqlTypes.NpgsqlDbType.Numeric);
                writer.Write(r.TotalAlunos, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.UE, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.DRE, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.Mes, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            }

            await writer.CompleteAsync();
        }

        public async Task ExcluirFrequenciaGlobal(int anoLetivo)
        {
            const string comando = @"delete from public.painel_educacional_registro_frequencia_agrupamento_escola where ano_letivo = @anoLetivo";

            await database.Conexao.ExecuteAsync(comando, new {anoLetivo});
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>> ObterFrequenciaGlobal(int anoLetivo, string codigoDre, string codigoUe)
        {
            var sql = $@"SELECT *, d.abreviacao as Dre FROM painel_educacional_registro_frequencia_agrupamento_escola p
                         INNER JOIN dre d on d.dre_id = p.codigo_dre 
                         WHERE 1=1";

            if (anoLetivo > 0)
                sql += " AND ano_letivo = @anoLetivo";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                sql += " AND p.codigo_dre = @codigoDre";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND p.codigo_ue = @codigoUe";

            return await database.QueryAsync<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>(sql, new {anoLetivo, codigoDre, codigoUe });
        }

        public async Task<bool> SalvarFrequenciaGlobal(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola> consolidacoes)
        {
            var sql = @"
                         INSERT INTO painel_educacional_registro_frequencia_agrupamento_mensal
                             (codigo_dre, codigo_ue, modalidade, ano_letivo, mes, total_aulas, total_faltas, percentual_frequencia, criado_por, criado_rf,  alterado_em, alterado_por, alterado_rf)
                         VALUES
                             (@CodigoDre, @CodigoUe, @Modalidade, @AnoLetivo, @Mes, @TotalAulas, @TotalFaltas, @PercentualFrequencia, @CriadoPor, @CriadoRF, @UltimaAtualizacao, @AlteradoPor, @AlteradoRF)";

            using var conn = database.Conexao;

            int rowsAffected = await conn.ExecuteAsync(sql, consolidacoes);

            return rowsAffected > 0;
        }
    }
}
