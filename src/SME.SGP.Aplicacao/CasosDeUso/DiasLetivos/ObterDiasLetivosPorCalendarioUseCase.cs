using MediatR;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasLetivosPorCalendarioUseCase : AbstractUseCase, IObterDiasLetivosPorCalendarioUseCase
    {
        public ObterDiasLetivosPorCalendarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DiasLetivosDto> Executar(FiltroDiasLetivosDTO param)
        {
            return await mediator.Send(new ObterQuantidadeDiasLetivosPorCalendarioQuery(param.TipoCalendarioId, param.DreId, param.UeId));
        }
    }
}
