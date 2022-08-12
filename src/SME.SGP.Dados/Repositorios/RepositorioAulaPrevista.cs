using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAulaPrevista : RepositorioBase<AulaPrevista>, IRepositorioAulaPrevista
    {
        public RepositorioAulaPrevista(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
