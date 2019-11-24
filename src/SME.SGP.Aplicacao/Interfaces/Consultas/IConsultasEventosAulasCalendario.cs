using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasEventosAulasCalendario
    {
        Task<IEnumerable<EventosAulasTipoDiaDto>> ObterEventoAulasDia(FiltroEventosAulasCalendarioDiaDto filtro);

        Task<IEnumerable<EventosAulasCalendarioDto>> ObterEventosAulasMensais(FiltroEventosAulasCalendarioDto filtro);

        Task<IEnumerable<EventosAulasTipoCalendarioDto>> ObterTipoEventosAulas(FiltroEventosAulasCalendarioMesDto filtro);
    }
}