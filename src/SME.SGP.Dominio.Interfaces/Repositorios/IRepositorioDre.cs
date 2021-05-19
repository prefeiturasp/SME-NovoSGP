using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDre
    {
        IEnumerable<Dre> ListarPorCodigos(string[] dresCodigos);

        IEnumerable<Dre> MaterializarCodigosDre(string[] idDres, out string[] naoEncontradas);

        Dre ObterPorCodigo(string codigo);

        Task<long> ObterIdDrePorCodigo(string codigo);

        Dre ObterPorId(long id);

        Task<Dre> ObterPorIdAsync(long id);

        IEnumerable<Dre> ObterTodas();

        Task<IEnumerable<Dre>> SincronizarAsync(IEnumerable<Dre> entidades);
        Task<string> ObterCodigoDREPorTurmaId(long turmaId);
        Task<string> ObterCodigoDREPorUEId(long ueId);
        Task<IEnumerable<long>> ObterIdsDresAsync();
    }
}