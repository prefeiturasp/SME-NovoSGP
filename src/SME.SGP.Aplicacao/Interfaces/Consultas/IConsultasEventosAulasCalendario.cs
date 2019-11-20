using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasEventosAulasCalendario
    {
        IEnumerable<EventosAulasCalendarioDto> ObterEventosAulasMensais(FiltroEventosAulasCalendarioDto filtro);

        IEnumerable<EventosAulasTipoCalendarioDto> ObterTipoEventosAulas(FiltroEventosAulasCalendarioMesDto filtro);
    }
}