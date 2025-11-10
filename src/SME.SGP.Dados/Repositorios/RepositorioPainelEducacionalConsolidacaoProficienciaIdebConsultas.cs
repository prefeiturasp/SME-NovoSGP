using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoProficienciaIdebConsultas : IRepositorioPainelEducacionalConsolidacaoProficienciaIdebConsultas
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPainelEducacionalConsolidacaoProficienciaIdebConsultas(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>> ObterConsolidacaoPorAnoVisaoUeAsync(int anoLetivo, string codigoUe)
        {
            const string query = @"
                SELECT ano_letivo as AnoLetivo,
                       serie_ano as SerieAno,
                       codigo_ue as CodigoUe,
                       nota,
                         AS ComponenteCurricularId,
                       proficiencia,
                       boletim
                  FROM painel_educacional_consolidacao_proficiencia_ideb_ue
                 WHERE ano_letivo = @anoLetivo 
                   and codigo_ue = @codigoUe
                 ORDER BY serie_ano, componente_curricular_id;";
            return await database.QueryAsync<PainelEducacionalConsolidacaoProficienciaIdebUe>(query, new { anoLetivo, codigoUe });
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>> ObterDadosParaConsolidarPorAnoAsync(int anoLetivo)
        {
            const string query = @"
                SELECT COALESCE(ideb.ano_letivo, proficienciaIdeb.ano_letivo) AS AnoLetivo,
                       COALESCE(ideb.serie_ano, proficienciaIdeb.serie_ano) AS SerieAno,
                       COALESCE(ideb.codigo_eol_escola, proficienciaIdeb.codigo_eol_escola) AS CodigoUe,
                       ideb.nota,
                       proficienciaIdeb.componente_curricular :: integer AS ComponenteCurricular,
                       proficienciaIdeb.proficiencia,
                       proficienciaIdeb.boletim
                  FROM ideb
                       FULL OUTER JOIN
                       proficiencia_ideb AS proficienciaIdeb ON ideb.ano_letivo = proficienciaIdeb.ano_letivo
                                             AND ideb.serie_ano = proficienciaIdeb.serie_ano
                                             AND ideb.codigo_eol_escola = proficienciaIdeb.codigo_eol_escola
                 WHERE COALESCE(ideb.ano_letivo, proficienciaIdeb.ano_letivo) = @anoLetivo;";

            return await database.QueryAsync<PainelEducacionalConsolidacaoProficienciaIdebUe>(query, new { anoLetivo });
        }
    }
}
