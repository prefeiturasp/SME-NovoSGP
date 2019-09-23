using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPrioridadePerfil : RepositorioBase<PrioridadePerfil>, IRepositorioPrioridadePerfil
    {
        public RepositorioPrioridadePerfil(ISgpContext conexao) : base(conexao)
        {
        }
    }
}