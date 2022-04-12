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
        public async Task<long> ExisteIdPendenciaDiarioBordo(long aulaId, long componenteCurricularId)
        {
            var sql = "select pendencia_id from pendencia_diario_bordo where aula_id = @aulaId and componente_curricular_id = @componenteCurricularId ";
            return await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { aulaId, componenteCurricularId }, commandTimeout: 60);
        }

    }
}
