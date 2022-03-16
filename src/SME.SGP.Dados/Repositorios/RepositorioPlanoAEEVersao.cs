using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEEVersao : RepositorioBase<PlanoAEEVersao>, IRepositorioPlanoAEEVersao
    {
        public RepositorioPlanoAEEVersao(ISgpContext database) : base(database)
        {
        }

        public async Task<int> ObterMaiorVersaoPlanoPorAlunoCodigo(string codigoAluno)
        {
            var query = @"select max(coalesce(numero,0)) from plano_aee pa 
                         left join plano_aee_versao pav on pa.id = pav.plano_aee_id 
                         where pa.aluno_codigo = @codigoAluno";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { codigoAluno });
        }

        public async Task<PlanoAEEVersaoDto> ObterUltimaVersaoPorPlanoId(long planoId)
        {
            var query = @"select pav.Id, pav.numero, pav.criado_em as CriadoEm 
                          from plano_aee_versao pav 
                         where pav.plano_aee_id = @planoId 
                         order by pav.numero desc";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAEEVersaoDto>(query, new { planoId });
        }

        public async Task<IEnumerable<PlanoAEEVersaoDto>> ObterVersoesPorCodigoEstudante(string codigoEstudante)
        {
            var query = @"select pav.Id, pav.numero, pav.criado_em as CriadoEm 
                          from plano_aee_versao pav 
                         inner join plano_aee pa on pa.id = pav.plano_aee_id 
                         where pa.aluno_codigo = @codigoEstudante";

            return await database.Conexao.QueryAsync<PlanoAEEVersaoDto>(query, new { codigoEstudante });
        }

        public async Task<IEnumerable<PlanoAEEVersaoDto>> ObterVersoesPorPlanoId(long planoId)
        {
            var query = @"select 
                            pav.Id, 
	                        pav.numero, 
	                        pav.criado_em as CriadoEm, 
	                        pav.criado_por as CriadoPor, 
	                        pav.criado_rf as CriadoRF, 
	                        pav.alterado_em as AlteradoEm,
	                        pav.alterado_por as AlteradoPor,
	                        pav.alterado_rf as AlteradoRf
                          from plano_aee_versao pav 
                          where pav.plano_aee_id = @planoId 
                          order by pav.numero desc";

            return await database.Conexao.QueryAsync<PlanoAEEVersaoDto>(query.ToString(), new { planoId });
        }

        public async Task<IEnumerable<PlanoAEEVersaoDto>> ObterVersoesSemReestruturacaoPorPlanoId(long planoId, long reestruturacaoId)
        {
            var query = @"select pav.Id, pav.numero, pav.criado_em as CriadoEm 
                          from plano_aee_versao pav 
                          left join plano_aee_reestruturacao pr on pr.plano_aee_versao_id = pav.id
                          where pav.plano_aee_id = @planoId 
                            and (pr.id is null or pr.id = @reestruturacaoId)
                          order by pav.numero desc";

            return await database.Conexao.QueryAsync<PlanoAEEVersaoDto>(query.ToString(), new { planoId, reestruturacaoId });
        }
    }
}
