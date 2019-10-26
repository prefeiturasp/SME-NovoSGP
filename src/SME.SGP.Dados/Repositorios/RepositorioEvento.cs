using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEvento : RepositorioBase<Evento>, IRepositorioEvento
    {
        public RepositorioEvento(ISgpContext conexao) : base(conexao)
        {
        }
    }
}