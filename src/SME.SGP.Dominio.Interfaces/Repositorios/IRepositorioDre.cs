using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDre
    {
        IEnumerable<Dre> ListarPorCodigos(string[] dresCodigos);

        IEnumerable<Dre> MaterializarCodigosDre(string[] idDres, out string[] naoEncontradas);

        Dre ObterPorCodigo(string codigo);

        Dre ObterPorId(long id);

        IEnumerable<Dre> ObterTodas();

        Task<IEnumerable<Dre>> SincronizarAsync(IEnumerable<Dre> entidades);
    }
}