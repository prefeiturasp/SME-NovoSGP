using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtribuicaoCJ : RepositorioBase<AtribuicaoCJ>, IRepositorioAtribuicaoCJ
    {
        public RepositorioAtribuicaoCJ(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<AtribuicaoCJ> ObterPorComponenteTurmaModalidadeUe(Modalidade modalidade, string turmaId, string ueId, long componenteCurricularId)
        {
            var query = @"
                        select
	                        *
                        from
	                        atribuicao_cj a
                        where
	                        a.modalidade = @modalidade
	                        and a.ue_id = @ueId
	                        and a.turma_id = @turmaId
	                        and a.componente_curricular_id = @componenteCurricularId";

            return (await database.Conexao.QueryFirstOrDefaultAsync<AtribuicaoCJ>(query, new
            {
                modalidade = (int)modalidade,
                ueId,
                turmaId,
                componenteCurricularId
            }));
        }
    }
}