using Polly;
using Polly.Registry;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
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

        public async Task<IEnumerable<PainelEducacionalIdep>> ObterTodosIdep()
        {
            var query = @"WITH base AS (
                 SELECT
                     ano_letivo,
                     CASE
                         WHEN serie_ano BETWEEN 1 AND 5 THEN 'anos_iniciais'
                         WHEN serie_ano BETWEEN 6 AND 9 THEN 'anos_finais'
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
                 -- REMOVA o filtro por ano para pegar TODOS!
                 -- WHERE ano_letivo = 2025
             ),
             faixas AS (
                 SELECT
                     ano_letivo,  -- ← Adicione ano_letivo aqui!
                     etapa,
                     faixa,
                     COUNT(*) AS quantidade
                 FROM base
                 WHERE etapa IS NOT NULL
                 GROUP BY ano_letivo, etapa, faixa  -- ← Agrupe por ano também
             ),
             medias AS (
                 SELECT
                     ano_letivo,  -- ← Adicione ano_letivo aqui!
                     etapa,
                     ROUND(AVG(nota), 2) AS media_geral
                 FROM base
                 WHERE etapa IS NOT NULL
                 GROUP BY ano_letivo, etapa  -- ← Agrupe por ano também
             ),
             ultima_data AS (
                 SELECT
                     ano_letivo,
                     MAX(criado_em) AS ultima_atualizacao
                 FROM base
                 GROUP BY ano_letivo
             )
             SELECT
                 f.ano_letivo,  -- ← Agora vem do faixas
                 f.etapa,
                 f.faixa,
                 f.quantidade,
                 m.media_geral,
                 u.ultima_atualizacao
             FROM faixas f
             JOIN medias m ON f.ano_letivo = m.ano_letivo AND f.etapa = m.etapa
             JOIN ultima_data u ON f.ano_letivo = u.ano_letivo
             ORDER BY f.ano_letivo, f.etapa, f.faixa;";

            return await policy.ExecuteAsync(() =>
            database.Conexao.QueryAsync<PainelEducacionalIdep>(query, new { anoLetivo })
            );
        }
    }
}
