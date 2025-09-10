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
    public class RepositorioIdepPainelEducacionalConsulta : RepositorioBase<Dominio.Entidades.PainelEducacionalIdep>, IRepositorioIdepPainelEducacionalConsulta
    {
        private readonly IAsyncPolicy policy;

        public RepositorioIdepPainelEducacionalConsulta(ISgpContext database,
            IReadOnlyPolicyRegistry<string> registry,
            IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            policy = registry.Get<IAsyncPolicy>(PoliticaPolly.SGP);
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterTodosIdep()
        {
            var query = @"WITH base AS (
                          SELECT
                              t1.ano_letivo,
                              CASE
                                  WHEN t1.serie_ano BETWEEN 1 AND 5 THEN 1 -- anos_iniciais
                                  WHEN t1.serie_ano BETWEEN 6 AND 9 THEN 2 -- anos_finais
                              END AS etapa,
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
                          FROM idep t1
                          INNER JOIN ue t2 ON t2.ue_id = t1.codigo_eol_escola
                          INNER JOIN dre t3 ON t3.id = t2.dre_id
                          WHERE t1.nota IS NOT NULL
                            AND t1.nota BETWEEN 0 AND 10
                      ),
                      faixas AS (
                          SELECT
                              ano_letivo, 
                              etapa,
                              faixa,
                              codigo_dre,
                              COUNT(*) AS quantidade
                          FROM base
                          WHERE etapa IS NOT NULL
                          GROUP BY ano_letivo, etapa, faixa, codigo_dre
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
                          f.etapa,
                          f.codigo_dre,
                          f.faixa,
                          f.quantidade,
                          m.media_geral,
                          u.ultima_atualizacao
                      FROM faixas f
                      JOIN medias m 
                          ON f.ano_letivo = m.ano_letivo 
                         AND f.etapa = m.etapa
                         AND f.codigo_dre = m.codigo_dre
                      JOIN ultima_data u 
                          ON f.ano_letivo = u.ano_letivo 
                         AND f.codigo_dre = u.codigo_dre
                      ORDER BY f.ano_letivo, f.etapa, f.codigo_dre, f.faixa;";
            
            return await policy.ExecuteAsync(() =>
            database.Conexao.QueryAsync<PainelEducacionalConsolidacaoIdep>(query)
            );
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterIdepPorAnoEtapa(int anoLetivo, int etapa)
        {
            var query = @"
            WITH base AS (
                SELECT
                    ano_letivo,
                    CASE
                        WHEN serie_ano BETWEEN 1 AND 5 THEN 1 -- anos_iniciais
                        WHEN serie_ano BETWEEN 6 AND 9 THEN 2 -- anos_finais
                    END AS etapa,
                    nota,
                    CASE
                        WHEN nota >= 0  AND nota < 1  THEN '0-1'
                        WHEN nota >= 1  AND nota < 2  THEN '1-2'
                        WHEN nota >= 2  AND nota < 3  THEN '2-3'
                        WHEN nota >= 3  AND nota < 4  THEN '3-4'
                        WHEN nota >= 4  AND nota < 5  THEN '4-5'
                        WHEN nota >= 5  AND nota < 6  THEN '5-6'
                        WHEN nota >= 6  AND nota < 7  THEN '6-7'
                        WHEN nota >= 7  AND nota < 8  THEN '7-8'
                        WHEN nota >= 8  AND nota < 9  THEN '8-9'
                        WHEN nota >= 9  AND nota <= 10 THEN '9-10'
                    END AS faixa,
                    criado_em
                FROM arquivo_idep
                WHERE nota IS NOT NULL
                AND nota BETWEEN 0 AND 10
            ),
            faixas AS (
                SELECT
                    ano_letivo, 
                    etapa,
                    faixa,
                    COUNT(*) AS quantidade
                FROM base
                WHERE etapa IS NOT NULL
                GROUP BY ano_letivo, etapa, faixa 
            ),
            medias AS (
                SELECT
                    ano_letivo, 
                    etapa,
                    ROUND(AVG(nota), 2) AS media_geral
                FROM base
                WHERE etapa IS NOT NULL
                GROUP BY ano_letivo, etapa 
            ),
            ultima_data AS (
                SELECT
                    ano_letivo,
                    MAX(criado_em) AS ultima_atualizacao
                FROM base
                GROUP BY ano_letivo
            )
            SELECT
                f.ano_letivo, 
                f.etapa,
                f.faixa,
                f.quantidade,
                m.media_geral,
                u.ultima_atualizacao
            FROM faixas f
            JOIN medias m ON f.ano_letivo = m.ano_letivo AND f.etapa = m.etapa
            JOIN ultima_data u ON f.ano_letivo = u.ano_letivo
            WHERE f.ano_letivo = @anoLetivo
              AND f.etapa = @etapa
            ORDER BY f.ano_letivo, f.etapa, f.faixa;";

            return await policy.ExecuteAsync(() =>
                database.Conexao.QueryAsync<PainelEducacionalConsolidacaoIdep>(query, new { anoLetivo, etapa })
            );
        }
    }
}