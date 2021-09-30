using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> ExistePendenciaPorPlano(long planoAeeId)
        {
            var query = @"select 1
	                        from pendencia_plano_aee ppaee
		                        inner join pendencia p
			                        on ppaee.pendencia_id = p.id
                          where ppaee.plano_aee_id = @planoAeeId and
	                            not p.excluido and
	                            p.situacao = @situacao;";

            var resultado = await database
               .Conexao.QueryFirstOrDefaultAsync<int>(query, new { planoAeeId, situacao = SituacaoPendencia.Pendente });

            return resultado > 0;
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
