using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalIdeb : IRepositorioPainelEducacionalIdeb
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalIdeb(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task LimparConsolidacao()
        {
            var sql = "DELETE FROM public.painel_educacional_consolidacao_ideb";
            await database.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<PainelEducacionalIdebDto>> ObterIdeb()
        {
            var query = @"SELECT 
                              t1.ano_letivo As AnoLetivo,
                              t1.serie_ano AS SerieAno,
                              t1.nota,
                              t1.criado_em,
                              t3.dre_id as CodigoDre,
                              t2.ue_id as CodigoUe
                          FROM ideb t1
                          INNER JOIN ue t2 ON t2.ue_id = t1.codigo_eol_escola
                          INNER JOIN dre t3 ON t3.id = t2.dre_id
                          WHERE t1.nota IS NOT NULL
                             AND t1.nota BETWEEN 0 AND 10
                             AND t1.serie_ano IS NOT NULL;";


            return await database.Conexao.QueryAsync<PainelEducacionalIdebDto>(query);
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoIdeb> indicadores)
        {

            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            // Inicia o COPY em modo binário
            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_ideb 
                    (ano_letivo, etapa, faixa, quantidade, media_geral, codigo_dre, codigo_ue, criado_em) 
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync((int)item.Etapa, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.Faixa, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.Quantidade, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.MediaGeral, NpgsqlTypes.NpgsqlDbType.Numeric);
                await writer.WriteAsync(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.TimestampTz);
            }

            await writer.CompleteAsync();
        }

    }
}
