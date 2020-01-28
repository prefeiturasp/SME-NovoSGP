using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDre
    {
        IEnumerable<Dre> ListarPorCodigos(string[] dresCodigos);

        IEnumerable<Dre> MaterializarCodigosDre(string[] idDres, out string[] naoEncontradas);

        Dre ObterPorCodigo(string dreId);

        IEnumerable<Dre> ObterTodas();

        IEnumerable<Dre> Sincronizar(IEnumerable<Dre> entidades);

        Dre ObterPorId(long id);
    }
}