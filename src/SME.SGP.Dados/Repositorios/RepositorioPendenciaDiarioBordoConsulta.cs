using Dapper;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaDiarioBordoConsulta : IRepositorioPendenciaDiarioBordoConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPendenciaDiarioBordoConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }
        public async Task<bool> ExisteIdPendenciaDiarioBordo(long pendenciaId)
        {
            return await database.Conexao.ExecuteAsync("select id from pendencia_diario_bordo where id = @pendenciaId", new { pendenciaId }, commandTimeout: 60) > 0;
        }

    }
}
