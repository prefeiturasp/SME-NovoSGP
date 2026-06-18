using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterAulasEventosProfessorCalendarioPorMesUseCase
    {
        Task<IEnumerable<EventoAulaDiaDto>> Executar(FiltroAulasEventosCalendarioDto filtro, long tipoCalendarioId, int mes);
    }
}

