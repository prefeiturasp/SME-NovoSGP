using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioAreaDoConhecimento : IRepositorioAreaDoConhecimento
    {
        private readonly ISgpContext database;

        public RepositorioAreaDoConhecimento(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<AreaDoConhecimentoDto>> ObterAreasDoConhecimentoPorComponentesCurriculares(long[] codigosComponentesCurriculares)
        {
            var query = @"select cac.id, cac.nome, cc.descricao_sgp as NomeComponenteCurricular, cc.id CodigoComponenteCurricular from componente_curricular cc
                          left join componente_curricular_area_conhecimento cac on cac.id = cc.area_conhecimento_id
                          where cc.id = ANY(@CodigosComponentesCurriculares)  ";

            var parametros = new { CodigosComponentesCurriculares = codigosComponentesCurriculares };

            return await database.Conexao.QueryAsync<AreaDoConhecimentoDto>(query, parametros);
        }
    }
}
