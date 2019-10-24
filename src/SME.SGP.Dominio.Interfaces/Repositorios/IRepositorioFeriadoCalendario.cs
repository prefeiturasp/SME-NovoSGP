using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFeriadoCalendario : IRepositorioBase<FeriadoCalendario>
    {
        IEnumerable<FeriadoCalendario> ObterFeriadosCalendario(FiltroFeriadoCalendarioDto filtro);

        bool VerificarRegistroExistente(long id, string nome);
    }
}