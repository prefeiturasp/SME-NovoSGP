using Polly;
using Polly.Registry;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioIdebPainelEducacionalConsulta : RepositorioBase<PainelEducacionalIdeb>, IRepositorioIdebPainelEducacionalConsulta
    {
        private readonly IAsyncPolicy policy;

        public RepositorioIdebPainelEducacionalConsulta(ISgpContext database,
            IReadOnlyPolicyRegistry<string> registry,
            IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            policy = registry.Get<IAsyncPolicy>(PoliticaPolly.SGP);
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoIdeb>> ObterTodosIdeb()
        {
            var query = @"WITH base AS (
                          SELECT
                              t1.ano_letivo,
                              t1.serie_ano AS serie,
                              t1.nota,
                              CASE
                                  WHEN t1.nota >= 0  AND t1.nota < 1  THEN '0-1'
                                  WHEN t1.nota >= 1  AND t1.nota < 2  THEN '1-2'
                                  WHEN t1.nota >= 2  AND t1.nota < 3  THEN '2-3'
                                  WHEN t1.nota >= 3  AND t1.nota < 4  THEN '3-4'
                                  WHEN t1.nota >= 4  AND t1.nota < 5  THEN '4-5'
                                  WHEN t1.nota >= 5  AND t1.nota < 6  THEN '5-6'
                                  WHEN t1.nota >= 6  AND t1.nota < 7  THEN '6-7'
                                  WHEN t1.nota >= 7  AND t1.nota < 8  THEN '7-8'
                                  WHEN t1.nota >= 8  AND t1.nota < 9  THEN '8-9'
                                  WHEN t1.nota >= 9  AND t1.nota <= 10 THEN '9-10'
                              END AS faixa,
                              t1.criado_em,
                              t3.dre_id as codigo_dre
                          FROM ideb t1
                          INNER JOIN ue t2 ON t2.ue_id = CAST(t1.codigo_eol_escola AS VARCHAR)
                          INNER JOIN dre t3 ON t3.id = t2.dre_id
                          WHERE t1.nota IS NOT NULL
                            AND t1.nota BETWEEN 0 AND 10
                            AND t1.codigo_eol_escola IS NOT NULL
                      ),
                      faixas AS (
                          SELECT
                              ano_letivo, 
                              serie,
                              faixa,
                              codigo_dre,
                              COUNT(*) AS quantidade
                          FROM base
                          WHERE etapa IS NOT NULL
                          GROUP BY ano_letivo, serie, faixa, codigo_dre
                      ),
                      medias AS (
                          SELECT
                              ano_letivo, 
                              etapa,
                              codigo_dre,
                              ROUND(AVG(nota), 2) AS media_geral
                          FROM base
                          WHERE etapa IS NOT NULL
                          GROUP BY ano_letivo, etapa, codigo_dre
                      ),
                      ultima_data AS (
                          SELECT
                              ano_letivo,
                              codigo_dre,
                              MAX(criado_em) AS ultima_atualizacao
                          FROM base
                          GROUP BY ano_letivo, codigo_dre
                      )
                      SELECT
                          f.ano_letivo, 
                          f.serie,
                          f.codigo_dre,
                          f.faixa,
                          f.quantidade,
                          m.media_geral,
                          u.ultima_atualizacao
                      FROM faixas f
                      JOIN medias m 
                          ON f.ano_letivo = m.ano_letivo 
                         AND f.serie = m.serie
                         AND f.codigo_dre = m.codigo_dre
                      JOIN ultima_data u 
                          ON f.ano_letivo = u.ano_letivo 
                         AND f.codigo_dre = u.codigo_dre
                      ORDER BY f.ano_letivo, f.serie, f.codigo_dre, f.faixa;";

            return await policy.ExecuteAsync(() =>
            database.Conexao.QueryAsync<PainelEducacionalConsolidacaoIdeb>(query)
            );
        }

        public async Task<IEnumerable<PainelEducacionalIdebDto>> ObterIdebPorAnoSerie(int anoLetivo, string serie, string codigoDre, string codigoUe)
        {
            if (!string.IsNullOrWhiteSpace(serie) && serie == "-99")
            {
                serie = null;
            }

            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre == "-99")
            {
                codigoDre = null;
            }

            if (!string.IsNullOrWhiteSpace(codigoUe) && codigoUe == "-99")
            {
                codigoUe = null;
            }

            var query = @"SELECT 
                          peci.ano_letivo AS AnoLetivo,
                          CAST(peci.etapa AS INTEGER) AS SerieAno,
                          peci.media_geral AS Nota,
                          peci.faixa AS Faixa,
                          peci.alterado_em AS CriadoEm,
                          CAST(peci.codigo_dre AS INTEGER) AS CodigoDre,
                          CASE 
                              WHEN peci.codigo_ue IS NOT NULL THEN CAST(peci.codigo_ue AS INTEGER)
                              ELSE 0 
                          END AS CodigoUe,
                          peci.quantidade AS Quantidade
                      FROM painel_educacional_consolidacao_ideb peci
                      WHERE 
                          (@serie IS NULL OR peci.etapa = @serie)
                          AND (@anoLetivo IS NULL OR @anoLetivo = -99 OR peci.ano_letivo = @anoLetivo)
                          AND (@codigoDre IS NULL OR peci.codigo_dre = @codigoDre)
                          AND (@codigoUe IS NULL OR peci.codigo_ue = @codigoUe)
                      ORDER BY 
                          peci.ano_letivo DESC,
                          peci.codigo_dre,
                          peci.codigo_ue,
                          peci.faixa;";

            return await policy.ExecuteAsync(() =>
                database.Conexao.QueryAsync<PainelEducacionalIdebDto>(query, new { anoLetivo, serie, codigoDre, codigoUe })
            );
        }
        public async Task<int?> ObterAnoMaisRecenteIdeb(string serie, string codigoDre, string codigoUe)
        {
            if (!string.IsNullOrWhiteSpace(serie) && serie == "-99")
            {
                serie = null;
            }

            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre == "-99")
            {
                codigoDre = null;
            }

            if (!string.IsNullOrWhiteSpace(codigoUe) && codigoUe == "-99")
            {
                codigoUe = null;
            }

            var query = @"SELECT MAX(peci.ano_letivo)
                      FROM painel_educacional_consolidacao_ideb peci
                      WHERE 
                          (@serie IS NULL OR peci.etapa = @serie)
                          AND (@codigoDre IS NULL OR peci.codigo_dre = @codigoDre)
                          AND (@codigoUe IS NULL OR peci.codigo_ue = @codigoUe)";

            return await policy.ExecuteAsync(() =>
                database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new { serie, codigoDre, codigoUe })
            );
        }
    }
}
