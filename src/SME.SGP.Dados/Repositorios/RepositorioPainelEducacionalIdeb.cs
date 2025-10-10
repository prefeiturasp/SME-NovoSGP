using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
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
                              i.ano_letivo As AnoLetivo,
                              i.serie_ano AS SerieAno,
                              i.nota,
                              i.criado_em,
                              d.dre_id as CodigoDre,
                              u.ue_id as CodigoUe
                          FROM ideb i
                          INNER JOIN ue u ON u.ue_id = i.codigo_eol_escola
                          INNER JOIN dre d ON d.id = u.dre_id
                          WHERE i.nota IS NOT NULL
                             AND i.nota BETWEEN 0 AND 10
                             AND i.serie_ano IS NOT NULL;";


            return await database.Conexao.QueryAsync<PainelEducacionalIdebDto>(query);
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoIdeb> indicadores)
        {
            var indicadoresUnicos = indicadores
                .GroupBy(i => new { i.AnoLetivo, i.Etapa, i.Faixa, i.CodigoDre, i.CodigoUe })
                .Select(g => g.First())
                .ToList();

            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            // Inicia o COPY em modo binário
            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_ideb 
                    (ano_letivo, etapa, faixa, quantidade, media_geral, codigo_dre, codigo_ue, criado_em) 
                FROM STDIN (FORMAT BINARY)
            ");

            await writer.CompleteAsync();
        }
    }
}
