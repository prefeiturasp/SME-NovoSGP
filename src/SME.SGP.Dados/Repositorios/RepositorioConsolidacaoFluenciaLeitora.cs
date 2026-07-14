using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFluenciaLeitora : RepositorioBase<ConsolidacaoPainelEducacionalFluenciaLeitora>, IRepositorioConsolidacaoFluenciaLeitora
    {
        private readonly IConfiguration configuration;

        public RepositorioConsolidacaoFluenciaLeitora(ISgpContext database, IServicoAuditoria servicoAuditoria, IConfiguration configuration) 
            : base(database, servicoAuditoria)
        {
            this.configuration = configuration;
        }

        public async Task ExcluirConsolidacaoFluenciaLeitora()
        {
            const string sql = @"DELETE FROM consolidacao_painel_educacional_fluencia_leitora";
            await database.Conexao.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<ConsolidacaoPainelEducacionalFluenciaLeitora>> ObterFluenciaLeitora(string codigoDre)
        {
            var sql = @"
                SELECT 
                    id, fluencia, descricao_fluencia, dre_codigo, percentual, quantidade_alunos, ano, periodo,
                    criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf
                FROM consolidacao_painel_educacional_fluencia_leitora 
                WHERE 1=1";

            var parametros = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre != "-99")
            {
                sql += " AND dre_codigo = @codigoDre";
                parametros.Add("@codigoDre", codigoDre);
            }

            return await database.Conexao.QueryAsync<ConsolidacaoPainelEducacionalFluenciaLeitora>(sql, parametros);
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> registros)
        {
            if (!registros.Any())
                return;

            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = await conn.BeginBinaryImportAsync(@"
                COPY consolidacao_painel_educacional_fluencia_leitora
                    (fluencia, descricao_fluencia, dre_codigo, percentual, quantidade_alunos, ano, periodo, criado_em, criado_por, criado_rf)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var registro in registros)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(registro.Fluencia ?? string.Empty, NpgsqlDbType.Varchar);
                await writer.WriteAsync(registro.DescricaoFluencia ?? string.Empty, NpgsqlDbType.Varchar);
                await writer.WriteAsync(registro.DreCodigo ?? string.Empty, NpgsqlDbType.Varchar);
                await writer.WriteAsync(registro.Percentual, NpgsqlDbType.Numeric);
                await writer.WriteAsync(registro.QuantidadeAluno, NpgsqlDbType.Integer);
                await writer.WriteAsync(registro.AnoLetivo, NpgsqlDbType.Integer);
                await writer.WriteAsync(registro.Periodo, NpgsqlDbType.Integer);
                await writer.WriteAsync(System.DateTime.Now, NpgsqlDbType.Timestamp);
                await writer.WriteAsync("SISTEMA", NpgsqlDbType.Varchar);
                await writer.WriteAsync("SISTEMA", NpgsqlDbType.Varchar);
            }

            await writer.CompleteAsync();
        }
    }
}