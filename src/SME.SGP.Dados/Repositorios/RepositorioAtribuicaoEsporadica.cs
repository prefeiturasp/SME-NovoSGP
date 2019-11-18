using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtribuicaoEsporadica : RepositorioBase<AtribuicaoEsporadica>, IRepositorioAtribuicaoEsporadica
    {
        public RepositorioAtribuicaoEsporadica(ISgpContext database) : base(database)
        {
        }
    }
}