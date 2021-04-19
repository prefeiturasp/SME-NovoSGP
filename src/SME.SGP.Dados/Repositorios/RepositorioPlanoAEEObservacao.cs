using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPlanoAEEObservacao : RepositorioBase<PlanoAEEObservacao>, IRepositorioPlanoAEEObservacao
    {
        public RepositorioPlanoAEEObservacao(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PlanoAEEObservacaoDto>> ObterObservacoesPlanoPorId(long planoId)
        {
            var query = "select * from plano_aee_observacao where not excluido and plano_aee_observacao_id = @planoId";

            return await database.Conexao.QueryAsync<PlanoAEEObservacaoDto>(query, new { planoId });
        }


    }
}
