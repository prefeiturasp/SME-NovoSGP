using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDreConsulta
    {
        IEnumerable<Dre> ListarPorCodigos(string[] dresCodigos);
        Task<IEnumerable<Dre>> ListarPorCodigosAsync(string[] dresCodigos);

        (IEnumerable<Dre> Dres, string[] CodigosDresNaoEncontrados) MaterializarCodigosDre(string[] idDres);

        Task<Dre> ObterPorCodigo(string codigo);

        Task<long> ObterIdDrePorCodigo(string codigo);

        Dre ObterPorId(long id);

        Task<Dre> ObterPorIdAsync(long id);

        Task<IEnumerable<Dre>> ObterTodas();

        Task<string> ObterCodigoDREPorTurmaId(long turmaId);
        Task<string> ObterCodigoDREPorUEId(long ueId);
        Task<IEnumerable<long>> ObterIdsDresAsync();
    }
}