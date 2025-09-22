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
