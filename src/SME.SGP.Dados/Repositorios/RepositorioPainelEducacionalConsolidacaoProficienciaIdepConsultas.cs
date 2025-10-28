using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoProficienciaIdepConsultas : IRepositorioPainelEducacionalConsolidacaoProficienciaIdepConsultas
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPainelEducacionalConsolidacaoProficienciaIdepConsultas(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe>> ObterDadosParaConsolidarPorAnoAsync(int anoLetivo)
        {
            const string query = @"
                SELECT COALESCE(idep.ano_letivo, proficienciaIdep.ano_letivo) AS AnoLetivo,
                       COALESCE(idep.serie_ano, proficienciaIdep.serie_ano) AS SerieAno,
                       COALESCE(idep.codigo_eol_escola, proficienciaIdep.codigo_eol_escola) AS CodigoUe,
                       idep.nota,
                       proficienciaIdep.componente_curricular :: integer AS ComponenteCurricular,
                       proficienciaIdep.proficiencia,
                       proficienciaIdep.boletim
                  FROM idep
                       FULL OUTER JOIN
                       proficiencia_idep AS proficienciaIdep ON idep.ano_letivo = proficienciaIdep.ano_letivo
                                             AND idep.serie_ano = proficienciaIdep.serie_ano
                                             AND idep.codigo_eol_escola = proficienciaIdep.codigo_eol_escola
                 WHERE COALESCE(idep.ano_letivo, proficienciaIdep.ano_letivo) = @anoLetivo;";

            return await database.QueryAsync<PainelEducacionalConsolidacaoProficienciaIdepUe>(query, new { anoLetivo });
        }
    }
}