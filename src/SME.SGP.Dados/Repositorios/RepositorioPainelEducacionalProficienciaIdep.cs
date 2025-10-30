using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalProficienciaIdep : IRepositorioPainelEducacionalProficienciaIdep
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPainelEducacionalProficienciaIdep(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdepUe>> ObterConsolidacaoPorAnoVisaoUeAsync(int limiteAnoLetivo, int anoLetivo, string codigoUe)
        {
            const string query = @"
                SELECT ano_letivo as AnoLetivo,
                       serie_ano as SerieAno,
                       codigo_ue as CodigoUe,
                       nota,
                       componente_curricular_id as ComponenteCurricular,
                       proficiencia,
                       boletim
                  FROM painel_educacional_consolidacao_proficiencia_idep_ue
                 WHERE ano_letivo >= @limiteAnoLetivo 
                   and codigo_ue = @codigoUe
                   and ((@anoLetivo = 0 AND ano_letivo < @anoAtual) OR ano_letivo = @anoLetivo)
                 ORDER BY serie_ano, componente_curricular_id;";
            return await database.QueryAsync<PainelEducacionalConsolidacaoProficienciaIdepUe>(query, new { limiteAnoLetivo, codigoUe, anoLetivo, anoAtual = DateTime.Now.Year });
        }
    }
}