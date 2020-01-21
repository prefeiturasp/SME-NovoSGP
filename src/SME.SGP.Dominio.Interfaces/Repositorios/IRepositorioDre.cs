using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDre
    {
        IEnumerable<Dre> ListarPorCodigos(string[] dresCodigos);

        IEnumerable<Dre> ObterTodas();

        IEnumerable<Dre> Sincronizar(IEnumerable<Dre> entidades);
        IEnumerable<Dre> MaterializarCodigosDre(string[] idDres, out string[] naoEncontradas);

        Dre ObterPorId(long id);
    }
}