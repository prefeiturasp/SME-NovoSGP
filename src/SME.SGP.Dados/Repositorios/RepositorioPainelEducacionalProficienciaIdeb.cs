using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalProficienciaIdeb : IRepositorioPainelEducacionalProficienciaIdeb
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPainelEducacionalProficienciaIdeb(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>> ObterConsolidacaoPorAnoVisaoUeAsync(int limiteAnoLetivo, int anoLetivo, string codigoUe)
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
                 WHERE ano_letivo >= @limiteAnoLetivo 
                   and codigo_ue = @codigoUe
                   and (@anoLetivo = 0 OR ano_letivo = @anoLetivo)
                 ORDER BY serie_ano, componente_curricular_id;";
            return await database.QueryAsync<PainelEducacionalConsolidacaoProficienciaIdebUe>(query, new { limiteAnoLetivo, codigoUe, anoLetivo });
        }
    }
}