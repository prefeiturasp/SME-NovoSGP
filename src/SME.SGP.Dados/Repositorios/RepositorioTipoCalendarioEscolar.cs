using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dados.Contexto;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoCalendarioEscolar : RepositorioBase<TipoCalendarioEscolar>, IRepositorioTipoCalendarioEscolar
    {
        public RepositorioTipoCalendarioEscolar(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
