using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioProficienciaIdebConsulta : IRepositorioProficienciaIdebConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioProficienciaIdebConsulta(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }


        public async Task<IEnumerable<ProficienciaIdeb>> ObterPorAnoLetivoCodigoUe(int anoLetivo, List<string> codigoUe)
        {
            var sql = @"select * from proficienciaIdeb
                        where ano_letivo = @anoLetivo
                        and codigo_eol_escola = any(@codigosUe)";

            return await database.Conexao.QueryAsync<ProficienciaIdeb>(sql, new { anoLetivo, codigoUe });
        }
    }
}
