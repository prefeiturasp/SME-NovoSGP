using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComponenteCurricular : RepositorioBase<ComponenteCurricular>, IRepositorioComponenteCurricular
    {
        public RepositorioComponenteCurricular(ISgpContext conexao) : base(conexao)
        {
        }
    }
}