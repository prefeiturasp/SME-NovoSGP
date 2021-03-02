using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaPlanoAEE : RepositorioBase<PendenciaPlanoAEE>, IRepositorioPendenciaPlanoAEE
    {
        public RepositorioPendenciaPlanoAEE(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<PendenciaPlanoAEE>> ObterPorPlanoId(long planoAEEId)
        {
            var query = @"select * 
                          from pendencia_plano_aee
                         where plano_aee_id = @planoAEEId ";

            return await database.QueryAsync<PendenciaPlanoAEE>(query, new { planoAEEId });
        }
    }
}
