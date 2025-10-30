using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioIdepPainelEducacionalConsulta : RepositorioBase<PainelEducacionalIdep>, IRepositorioIdepPainelEducacionalConsulta
    {

        public RepositorioIdepPainelEducacionalConsulta(ISgpContext database,
            IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        { }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterTodosIdep()
        {
            var query = @"
                            WITH base AS (
                                SELECT
                                    t1.ano_letivo,
                                    t1.serie_ano AS etapa,
                                    t1.nota,
                                    CASE
                                        WHEN t1.nota >= 0  AND t1.nota < 0.99 THEN '0-1'
                                        WHEN t1.nota >= 1  AND t1.nota < 1.99 THEN '1-2'
                                        WHEN t1.nota >= 2  AND t1.nota < 2.99 THEN '2-3'
                                        WHEN t1.nota >= 3  AND t1.nota < 3.99 THEN '3-4'
                                        WHEN t1.nota >= 4  AND t1.nota < 4.99 THEN '4-5'
                                        WHEN t1.nota >= 5  AND t1.nota < 5.99 THEN '5-6'
                                        WHEN t1.nota >= 6  AND t1.nota < 6.99 THEN '6-7'
                                        WHEN t1.nota >= 7  AND t1.nota < 7.99 THEN '7-8'
                                        WHEN t1.nota >= 8  AND t1.nota < 8.99 THEN '8-9'
                                        WHEN t1.nota >= 9  AND t1.nota <= 10 THEN '9-10'
                                    END AS faixa,
                                    t1.criado_em,
                                    t2.ue_id AS codigo_ue,
                                    t3.dre_id AS codigo_dre
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
                                    codigo_ue,
                                    COUNT(*) AS quantidade
                                FROM base
                                WHERE etapa IS NOT NULL
                                GROUP BY ano_letivo, etapa, faixa, codigo_dre, codigo_ue
                            ),
                            medias AS (
                                SELECT
                                    ano_letivo,
                                    etapa,
                                    codigo_dre,
                                    codigo_ue,
                                    ROUND(AVG(nota), 2) AS media_geral
                                FROM base
                                WHERE etapa IS NOT NULL
                                GROUP BY ano_letivo, etapa, codigo_dre, codigo_ue
                            ),
                            ultima_data AS (
                                SELECT
                                    ano_letivo,
                                    codigo_dre,
                                    codigo_ue,
                                    MAX(criado_em) AS ultima_atualizacao
                                FROM base
                                GROUP BY ano_letivo, codigo_dre, codigo_ue
                            )
                            SELECT
                                f.ano_letivo AS AnoLetivo,
                                f.codigo_dre AS CodigoDre,
                                f.codigo_ue AS CodigoUe,
                                f.etapa AS Etapa,
                                f.faixa AS Faixa,
                                f.quantidade AS Quantidade,
                                m.media_geral AS MediaGeral,
                                u.ultima_atualizacao AS UltimaAtualizacao
                            FROM faixas f
                            JOIN medias m 
                                ON f.ano_letivo = m.ano_letivo 
                               AND f.etapa = m.etapa
                               AND f.codigo_dre = m.codigo_dre
                               AND f.codigo_ue = m.codigo_ue
                            JOIN ultima_data u 
                                ON f.ano_letivo = u.ano_letivo 
                               AND f.codigo_dre = u.codigo_dre
                               AND f.codigo_ue = u.codigo_ue
                            ORDER BY f.ano_letivo, f.etapa, f.codigo_dre, f.codigo_ue, f.faixa;";

            return await database.Conexao.QueryAsync<PainelEducacionalConsolidacaoIdep>(query);
        }

        public async Task<IEnumerable<PainelEducacionalIdepDto>> ObterIdepPorAnoEtapa(int anoLetivo, int etapa, string codigoDre)
        {
                var query = @"SELECT 
                                  peci.ano_letivo,
                                  peci.codigo_dre,
                                  peci.etapa,
                                  peci.faixa,
                                  peci.quantidade,
                                  peci.media_geral,
                                  peci.criado_em AS ultima_atualizacao
                              FROM painel_educacional_consolidacao_idep peci
                              WHERE peci.ano_letivo = @anoLetivo
                              AND peci.etapa = @etapa";

                if (!string.IsNullOrWhiteSpace(codigoDre))
                    query += " AND peci.codigo_dre = @codigoDre ";

                query += @" ORDER BY 
                                  peci.ano_letivo DESC,
                                  peci.codigo_dre,
                                  peci.faixa;";

                return await database.Conexao.QueryAsync<PainelEducacionalIdepDto>(query, new { anoLetivo, etapa, codigoDre });
        }
    }
}
