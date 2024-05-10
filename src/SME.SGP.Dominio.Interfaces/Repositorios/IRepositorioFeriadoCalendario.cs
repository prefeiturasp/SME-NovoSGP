using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFeriadoCalendario : IRepositorioBase<FeriadoCalendario>
    {
        Task<IEnumerable<FeriadoCalendario>> ObterFeriadosCalendario(FiltroFeriadoCalendarioDto filtro);

        bool VerificarRegistroExistente(long id, string nome);
    }
}