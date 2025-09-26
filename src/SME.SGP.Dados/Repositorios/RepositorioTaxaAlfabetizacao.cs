using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.TaxaAlfabetizacao;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTaxaAlfabetizacao : RepositorioBase<TaxaAlfabetizacao>, IRepositorioTaxaAlfabetizacao
    {
        private readonly IConfiguration configuration;
        public RepositorioTaxaAlfabetizacao(ISgpContext database, IServicoAuditoria servicoAuditoria, IConfiguration configuration) : base(database, servicoAuditoria)
        {
            this.configuration = configuration;
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

        public async Task<TaxaAlfabetizacao> ObterRegistroAlfabetizacaoAsync(int anoLetivo, string codigoEOLEscola)
        {
            string query = @"select * from taxa_alfabetizacao
                             where ano_letivo = @anoLetivo
                             and codigo_eol_escola = @codigoEOLEscola;";

            return await database.Conexao.QueryFirstOrDefaultAsync<TaxaAlfabetizacao>(query, new
            {
                anoLetivo,
                codigoEOLEscola
            });
        }

        public async Task<IEnumerable<TaxaAlfabetizacaoDto>> ObterTaxaAlfabetizacaoAsync(int anoLetivo, string codigoDre, string codigoUe)
        {
            string query = @"select 
                                    d.dre_id as CodigoDre,  
                                    u.ue_id as CodigoUe,
                                    tx.ano_letivo as AnoLetivo,      
                                    tx.codigo_eol_escola as CodigoEOLEscola,
                                    tx.taxa as Taxa 
                                from Taxa_alfabetizacao tx
                                inner join ue u on tx.codigo_eol_escola = u.ue_id 
                                inner join dre d on d.id = u.dre_id
                            where 1 = 1";

            if (anoLetivo > 0)
                query += " and tx.ano_letivo = @anoLetivo";

            if (!string.IsNullOrEmpty(codigoDre))
                query += " and d.dre_id = @codigoDre";

            if (!string.IsNullOrEmpty(codigoUe))
                query += " and d.ue_id = @codigoUe";

            return await database.Conexao.QueryAsync<TaxaAlfabetizacaoDto>(query, new
            {
                anoLetivo,
                codigoDre,
                codigoUe
            });
        }
    }
}
