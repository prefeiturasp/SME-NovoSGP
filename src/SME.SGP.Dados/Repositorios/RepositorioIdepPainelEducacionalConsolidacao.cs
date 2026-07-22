using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio; 

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioIdepPainelEducacionalConsolidacao : PainelEducacionalConsolidacaoIdep, IRepositorioIdepPainelEducacionalConsolidacao
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;
        public RepositorioIdepPainelEducacionalConsolidacao(ISgpContext database, IConfiguration configuration)
        {
            this.database = database;
            this.configuration = configuration;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoIdep> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = await conn.BeginBinaryImportAsync(@"
                COPY painel_educacional_consolidacao_idep 
                    (ano_letivo, codigo_dre, codigo_ue, etapa, faixa, quantidade, media_geral, criado_em) 
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync((int)item.Etapa, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.Faixa, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.Quantidade, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.MediaGeral, NpgsqlTypes.NpgsqlDbType.Numeric);
                await writer.WriteAsync(DateTimeExtension.EnsureUnspecified(item.CriadoEm), NpgsqlTypes.NpgsqlDbType.Timestamp);
            }

            await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao()
        {
            const string sql = "DELETE FROM painel_educacional_consolidacao_idep";
            await database.ExecuteAsync(sql);
        }
    }
}