using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEEVersao : RepositorioBase<PlanoAEEVersao>, IRepositorioPlanoAEEVersao
    {
        public RepositorioPlanoAEEVersao(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PlanoAEEVersaoDto>> ObterVersoesPorPlanoId(long planoId)
        {
            var query = @"select pav.Id, pav.numero 
                          from plano_aee_versao pav 
                         where pav.plano_aee_id = @planoId";

            return await database.Conexao.QueryAsync<PlanoAEEVersaoDto>(query, new { planoId });
        }
    }
}
