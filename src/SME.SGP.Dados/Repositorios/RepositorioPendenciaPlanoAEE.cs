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

        public async Task<PendenciaPlanoAEE> ObterPorPlanoId(long planoAEEId)
        {
            var query = @"select * 
                          from pendencia_plano_aee
                         where plano_aee_id = @planoAEEId ";

            return await database.QueryFirstOrDefaultAsync<PendenciaPlanoAEE>(query, new { planoAEEId });
        }
    }
}
