using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalProficienciaIdep : IRepositorioPainelEducacionalProficienciaIdep
    {
        private readonly ISgpContext database;
        public RepositorioPainelEducacionalProficienciaIdep(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<ProficienciaIdepAgrupadaDto>> ObterProficienciaIdep(int anoLetivo, string codigoUe)
        {
            int anoMinimo = 2019;

            var sql = $@"SELECT ano_letivo AS AnoLetivo,
                           componente_curricular AS ComponenteCurricular,
                           boletim AS Boletim,
                           serie_ano AS EtapaEnsino,
                           AVG(proficiencia) AS ProficienciaMedia
                           FROM proficiencia_idep
                           WHERE ano_letivo >= @anoMinimo";

            if (anoLetivo > 0)
                sql += " AND ano_letivo = @anoLetivo ";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND codigo_eol_escola = @codigoUe ";

            sql += $@"GROUP BY ano_letivo, componente_curricular, boletim, serie_ano";

            return await database.QueryAsync<ProficienciaIdepAgrupadaDto>(sql, new { anoMinimo, anoLetivo, codigoUe });
        }
    }
}
