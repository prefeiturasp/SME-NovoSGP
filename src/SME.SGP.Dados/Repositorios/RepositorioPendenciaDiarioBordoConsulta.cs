using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

        public async Task<IEnumerable<PendenciaDiarioBordoDescricaoDto>> ObterPendenciasDiarioPorPendencia(long pendenciaId, string codigoRf)
        {
            var query = @"select a.data_aula as DataAula, coalesce(cc.descricao_infantil , cc.descricao_sgp, cc.descricao) as ComponenteCurricular
                           from pendencia_diario_bordo pdb
                          inner join aula a on a.id = pdb.aula_id
                          inner join componente_curricular cc on cc.id = pdb.componente_curricular_id 
                          where pdb.pendencia_id = @pendenciaId and pdb.professor_rf = @codigoRf
                          order by a.data_aula desc";

            return await database.Conexao.QueryAsync<PendenciaDiarioBordoDescricaoDto>(query, new { pendenciaId, codigoRf });
        }
    }
}
