using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDevolutiva: RepositorioBase<Devolutiva>, IRepositorioDevolutiva
    {
        public RepositorioDevolutiva(ISgpContext conexao) : base(conexao) { }

        public async Task<DateTime> ObterUltimaDataDevolutiva(string turmaCodigo, long componenteCurricularCodigo)
        {
            var query = @"select d.periodo_fim
                          from devolutiva d
                         inner join diario_bordo db on db.devolutiva_id = d.id
                         inner join aula a on a.id = db.aula_id
                         where a.turma_id = @turmaCodigo
                           and d.componente_curricular_codigo = @componenteCurricularCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<DateTime>(query, new { turmaCodigo, componenteCurricularCodigo });
        }
    }
}
