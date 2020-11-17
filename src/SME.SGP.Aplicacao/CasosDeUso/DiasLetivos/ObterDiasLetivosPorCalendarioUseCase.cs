using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dto;
using SME.SGP.Infra;

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
