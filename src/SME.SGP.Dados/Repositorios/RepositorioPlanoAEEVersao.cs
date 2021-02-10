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

        public async Task<int> ObterMaiorVersaoPlanoPorAlunoCodigo(string codigoAluno)
        {
            var query = @"select max(coalesce(numero,0)) from plano_aee pa 
                         left join plano_aee_versao pav on pa.id = pav.plano_aee_id 
                         where pa.aluno_codigo = @codigoAluno";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { codigoAluno });
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
            var query = @"select pav.Id, pav.numero, pav.criado_em as CriadoEm 
                          from plano_aee_versao pav 
                         where pav.plano_aee_id = @planoId order by pav.numero desc";

            return await database.Conexao.QueryAsync<PlanoAEEVersaoDto>(query, new { planoId });
        }
    }
}
