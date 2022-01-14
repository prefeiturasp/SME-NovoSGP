using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDreConsulta
    {
        IEnumerable<Dre> ListarPorCodigos(string[] dresCodigos);

        Tuple<IEnumerable<Dre>, string[]> MaterializarCodigosDre(string[] idDres);

        Dre ObterPorCodigo(string codigo);

        Task<long> ObterIdDrePorCodigo(string codigo);

        Dre ObterPorId(long id);

        Task<Dre> ObterPorIdAsync(long id);

        IEnumerable<Dre> ObterTodas();

        Task<string> ObterCodigoDREPorTurmaId(long turmaId);
        Task<string> ObterCodigoDREPorUEId(long ueId);
        Task<IEnumerable<long>> ObterIdsDresAsync();
    }
}