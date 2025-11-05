using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoFluenciaLeitoraUe : IRepositorioPainelEducacionalConsolidacaoFluenciaLeitoraUe
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalConsolidacaoFluenciaLeitoraUe(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoFluenciaLeitoraUe> indicadores)
        {
                await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
                await conn.OpenAsync();

                await using var writer = conn.BeginBinaryImport(@"
                    COPY painel_educacional_consolidacao_fluencia_leitora_ue
                        (ano_letivo, codigo_dre, codigo_ue, codigo_turma, turma, alunos_previstos, alunos_avaliados,
                         tipo_avaliacao, pre_leitor_total, fluencia, quantidade_aluno_fluencia,
                         percentual_fluencia, criado_em)
                    FROM STDIN (FORMAT BINARY)
                ");

                foreach (var item in indicadores)
                {
                    await writer.StartRowAsync();
                    await writer.WriteAsync(item.AnoLetivo, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
                    await writer.WriteAsync(item.CodigoUe, NpgsqlDbType.Varchar);
                    await writer.WriteAsync(item.CodigoTurma, NpgsqlDbType.Varchar);
                    await writer.WriteAsync(item.Turma, NpgsqlDbType.Varchar);
                    await writer.WriteAsync(item.AlunosPrevistos, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.AlunosAvaliados, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.TipoAvaliacao, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.PreLeitorTotal, NpgsqlDbType.Integer);
                    await writer.WriteAsync((int)item.Fluencia, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.QuantidadeAlunoFluencia, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.PercentualFluencia, NpgsqlDbType.Numeric);
                    await writer.WriteAsync(item.CriadoEm, NpgsqlDbType.TimestampTz);
                }
                await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao(int ano)
        {
            var sql = @"DELETE FROM painel_educacional_consolidacao_fluencia_leitora_ue WHERE ano_letivo >= @ano;
                        SELECT setval(
                            pg_get_serial_sequence('painel_educacional_consolidacao_fluencia_leitora_ue', 'id'),
                            (SELECT MAX(id) FROM painel_educacional_consolidacao_fluencia_leitora_ue)
                        );
                        ";

            await database.ExecuteAsync(sql, new { ano });
        }

        public async Task<IEnumerable<ConsolidacaoFluenciaLeitoraUeDto>> ObterDadosParaConsolidarFluenciaLeitoraUe(int anoLetivo)
        {
            var sql = @"SELECT 
                            t4.dre_id AS CodigoDre
                          ,	t3.ue_id AS CodigoUe
                          , t1.codigo_eol_turma as CodigoTurma
                          , t2.nome AS Turma  
                          , t1.tipo_avaliacao AS tipoAvaliacao
                          , t1.fluencia AS Fluencia
                        FROM fluencia_leitora t1 
                        INNER JOIN turma t2 on (t1.codigo_eol_turma = t2.turma_id) 
                        INNER JOIN ue t3 on (t3.id = t2.ue_id)
                        INNER JOIN dre t4 on (t4.id = t3.dre_id)
                        WHERE t1.ano_letivo = @anoLetivo AND t2.ano = '2';";

            return await database.QueryAsync<ConsolidacaoFluenciaLeitoraUeDto>(sql, new { anoLetivo });
        }
    }
}
