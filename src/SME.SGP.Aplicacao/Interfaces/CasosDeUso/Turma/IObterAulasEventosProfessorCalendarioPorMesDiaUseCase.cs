using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterAulasEventosProfessorCalendarioPorMesDiaUseCase
    {
        Task<EventosAulasNoDiaCalendarioDto> Executar(FiltroAulasEventosCalendarioDto filtro, long tipoCalendarioId, int mes, int dia, int anoLetivo);
    }
}
