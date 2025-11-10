using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioIdebPainelEducacionalConsulta : IRepositorioIdebPainelEducacionalConsulta
    {
        private readonly ISgpContext database;
        public RepositorioIdebPainelEducacionalConsulta(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalIdebDto>> ObterIdebPorAnoSerie(int anoLetivo, int serie, string codigoDre, string codigoUe)
        {
            string query = @"SELECT 
                                  peci.ano_letivo AS AnoLetivo,
                                  peci.etapa AS SerieAno,
                                  peci.media_geral AS Nota,
                                  peci.faixa AS Faixa,
                                  peci.criado_Em AS CriadoEm,
                                  peci.codigo_dre AS CodigoDre,
                                  peci.codigo_ue AS CodigoUe,
                                  peci.quantidade AS Quantidade
                              FROM painel_educacional_consolidacao_ideb peci
                                    where peci.ano_letivo = @anoLetivo
                                    AND peci.etapa = @serie";

            if (!string.IsNullOrEmpty(codigoDre))
                query += " and peci.codigo_dre = @codigoDre";

            if (!string.IsNullOrEmpty(codigoUe))
                query += " and peci.codigo_ue = @codigoUe";

            return await database.Conexao.QueryAsync<PainelEducacionalIdebDto>(query, new { anoLetivo, serie, codigoDre, codigoUe }
            );
        }

        public async Task<int?> ObterAnoMaisRecenteIdeb(int serie, string codigoDre, string codigoUe)
        {
            var query = @"SELECT MAX(peci.ano_letivo)
                      FROM painel_educacional_consolidacao_ideb peci
                      WHERE 
                          peci.etapa = @serie";

            if (!string.IsNullOrEmpty(codigoDre))
                query += " and peci.codigo_dre = @codigoDre";

            if (!string.IsNullOrEmpty(codigoUe))
                query += " and peci.codigo_ue = @codigoUe";



            return await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new { serie, codigoDre, codigoUe });
        }
    }
}
