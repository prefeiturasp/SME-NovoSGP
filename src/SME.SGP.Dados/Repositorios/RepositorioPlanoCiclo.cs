using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoCiclo : RepositorioBase<PlanoCiclo>, IRepositorioPlanoCiclo
    {
        public RepositorioPlanoCiclo(SgpContext conexao) : base(conexao)
        {
        }
    }
}