using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEEReestruturacao : RepositorioBase<PlanoAEEReestruturacao>, IRepositorioPlanoAEEReestruturacao
    {
        public RepositorioPlanoAEEReestruturacao(ISgpContext database) : base(database)
        {

        }

        public async Task<IEnumerable<PlanoAEEReestruturacaoDto>> ObterRestruturacoesPorPlanoAEEId(long planoId)
        {
            var query = @"select 
                                par.id, 
                                pav.criado_em as Data,
                                par.semestre as Semestre,
                                par.plano_aee_versao_id as VersaoId,
                                pav.numero as Versao,
                                descricao as Descricao
                            from plano_aee_reestruturacao par 
                            inner join plano_aee_versao pav on pav.id = par.plano_aee_versao_id
                            where pav.plano_aee_id = @planoId 
                            order by pav.criado_em ";

            return await database.Conexao.QueryAsync<PlanoAEEReestruturacaoDto>(query.ToString(), new { planoId });
        }

    }
}
