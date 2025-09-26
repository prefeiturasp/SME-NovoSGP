using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.TaxaAlfabetizacao;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao : IRepositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoTaxaAlfabetizacao> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            // Inicia o COPY em modo binário
            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_taxa_alfabetizacao 
                    (codigo_dre, codigo_ue, ano_letivo, taxa, criado_em) 
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.TaxaAlfabetizacao, NpgsqlTypes.NpgsqlDbType.Numeric);
                await writer.WriteAsync(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.TimestampTz);
            }

            await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao()
        {
            var sql = "DELETE FROM painel_educacional_consolidacao_taxa_alfabetizacao";
            await database.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoTaxaAlfabetizacao>> ObterConsolidacaoAsync(int anoLetivo, string codigoDre, string codigoUe)
        {
            string query = @"select 
                                    tx.codigo_dre as CodigoDre,  
                                    tx.codigo_ue as CodigoUe,
                                    tx.ano_letivo as AnoLetivo,      
                                    tx.taxa as TaxaAlfabetizacao 
                                from painel_educacional_consolidacao_taxa_alfabetizacao tx
                            where tx.ano_letivo = @anoLetivo";

            if (!string.IsNullOrEmpty(codigoDre))
                query += " and tx.dre_id = @codigoDre";

            if (!string.IsNullOrEmpty(codigoUe))
                query += " and tx.ue_id = @codigoUe";

            return await database.Conexao.QueryAsync<PainelEducacionalConsolidacaoTaxaAlfabetizacao>(query, new
            {
                anoLetivo,
                codigoDre,
                codigoUe
            });
        }
    }
}
