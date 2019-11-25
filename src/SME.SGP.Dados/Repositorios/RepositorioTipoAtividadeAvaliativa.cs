using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoAtividadeAvaliativa : RepositorioBase<TipoAtividadeAvaliativa>, IRepositorioTipoAtividadeAvaliativa
    {
        public RepositorioTipoAtividadeAvaliativa(ISgpContext conexao) : base(conexao)
        {
        }
    }
}