using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioProficienciaIdepConsulta : IRepositorioProficienciaIdepConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioProficienciaIdepConsulta(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<ProficienciaIdep>> ObterPorAnoLetivoCodigoUe(int anoLetivo, List<string> codigoUe)
        {
            var sql = @"select * from proficienciaIdep
                        where ano_letivo = @anoLetivo
                        and codigo_eol_escola = any(@codigosUe)";

            return await database.Conexao.QueryAsync<ProficienciaIdep>(sql, new { anoLetivo, codigoUe });
        }
    }
}
