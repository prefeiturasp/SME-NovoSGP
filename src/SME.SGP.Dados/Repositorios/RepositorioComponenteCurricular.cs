using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioComponenteCurricular : IRepositorioComponenteCurricular
    {
        private readonly ISgpContext database;

        public RepositorioComponenteCurricular(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task<IEnumerable<ComponenteCurricular>> ObterPorIdsAsync(long[] ids)
        {
            var query = @"select distinct cc.*, 
                            (select id from componente_curricular_jurema ccj where ccj.codigo_eol = cc.id limit 1) is not null TemCurriculoCidade
                            from componente_curricular cc;
                         cc.id = any(@ids)";

            return await database.Conexao.QueryAsync<ComponenteCurricular>(query, new { ids });
        }
    }
}
