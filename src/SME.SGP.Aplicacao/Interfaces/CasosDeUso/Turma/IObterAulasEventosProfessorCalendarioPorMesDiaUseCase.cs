using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterAulasEventosProfessorCalendarioPorMesDiaUseCase
    {
        Task<EventosAulasNoDiaCalendarioDto> Executar(FiltroAulasEventosCalendarioDto filtro, long tipoCalendarioId, int mes, int dia, int anoLetivo);
    }
}
