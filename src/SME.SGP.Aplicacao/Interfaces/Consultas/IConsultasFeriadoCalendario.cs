using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFeriadoCalendario
    {
        FeriadoCalendarioDto BuscarPorId(long id);

        IEnumerable<FeriadoCalendarioDto> Listar(FiltroFeriadoCalendarioDto filtro);
    }
}