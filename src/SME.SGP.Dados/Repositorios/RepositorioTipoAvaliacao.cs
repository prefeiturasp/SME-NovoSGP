using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoAvaliacao : RepositorioBase<TipoAvaliacao>, IRepositorioTipoAvaliacao
    {
        public RepositorioTipoAvaliacao(ISgpContext conexao) : base(conexao)
        {
        }
    }
}