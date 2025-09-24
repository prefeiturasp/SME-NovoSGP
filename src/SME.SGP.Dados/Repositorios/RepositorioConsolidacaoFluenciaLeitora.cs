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
        
        public async Task<IEnumerable<ConsolidacaoPainelEducacionalFluenciaLeitora>> ObterFluenciaLeitora(string codigoDre, string codigoUe)
        {
            var sql = @"
                SELECT 
                    id, fluencia, descricao_fluencia, percentual, quantidade_alunos, ano, periodo,
                    criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf
                FROM consolidacao_painel_educacional_fluencia_leitora 
                WHERE 1=1";

            var parametros = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(codigoDre))
            {
                sql += " AND dre_codigo = @codigoDre";
                parametros.Add("@codigoDre", codigoDre);
            }

            if (!string.IsNullOrWhiteSpace(codigoUe))
            {
                sql += " AND ue_codigo = @codigoUe";
                parametros.Add("@codigoUe", codigoUe);
            }

            return await database.Conexao.QueryAsync<ConsolidacaoPainelEducacionalFluenciaLeitora>(sql, parametros);
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> registros)
        {
            if (!registros.Any())
                return;

            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY consolidacao_painel_educacional_fluencia_leitora
                    (fluencia, descricao_fluencia, percentual, quantidade_alunos, ano, periodo, criado_em, criado_por, criado_rf)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var registro in registros)
            {
                writer.StartRow();
                writer.Write(registro.Fluencia ?? string.Empty, NpgsqlDbType.Varchar);
                writer.Write(registro.DescricaoFluencia ?? string.Empty, NpgsqlDbType.Varchar);
                writer.Write(registro.Percentual, NpgsqlDbType.Numeric);
                writer.Write(registro.QuantidadeAluno, NpgsqlDbType.Integer);
                writer.Write(registro.AnoLetivo, NpgsqlDbType.Integer);
                writer.Write(registro.Periodo, NpgsqlDbType.Integer);
                writer.Write(System.DateTime.Now, NpgsqlDbType.Timestamp);
                writer.Write("SISTEMA", NpgsqlDbType.Varchar);
                writer.Write("SISTEMA", NpgsqlDbType.Varchar);
            }

            await writer.CompleteAsync();
        }
    }
}