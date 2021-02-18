using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEEReestruturacao : RepositorioBase<PlanoAEEReestruturacao>, IRepositorioPlanoAEEReestruturacao
    {
        public RepositorioPlanoAEEReestruturacao(ISgpContext database) : base(database)
        {
        }

        public async Task<bool> ExisteReestruturacaoParaVersao(long versaoId, long reestruturacaoId)
        {
            var query = @"select 1
                          from plano_aee_reestruturacao 
                         where plano_aee_versao_id = @versaoId
                           and id <> @reestruturacaoId";

            return (await database.Conexao.QueryAsync<int>(query, new { versaoId, reestruturacaoId })).Any();
        }
    }
}
