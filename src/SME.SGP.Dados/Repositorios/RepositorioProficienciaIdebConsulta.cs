using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
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


        public async Task<ProficienciaIdeb> ObterPorAnoLetivoCodigoUe(int anoLetivo, string codigoUe)
        {
            var sql = @"select * from proficienciaIdeb
                        where ano_letivo = @anoLetivo
                        and codigo_eol_escola = @codigoUe";

            return await database.Conexao.QueryFirstOrDefaultAsync<ProficienciaIdeb>(sql, new { anoLetivo, codigoUe });
        }
    }
}
