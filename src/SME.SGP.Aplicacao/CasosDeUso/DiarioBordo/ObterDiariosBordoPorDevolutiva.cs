using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosBordoPorDevolutiva : AbstractUseCase, IObterDiariosBordoPorDevolutiva
    {
        public ObterDiariosBordoPorDevolutiva(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> Executar(long devolutivaId)
            => await mediator.Send(new ObterDiariosBordoPorDevolutivaQuery(devolutivaId));
    }
}
