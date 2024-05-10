using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDevolutivaUseCase : AbstractUseCase, IExcluirDevolutivaUseCase
    {
        public ExcluirDevolutivaUseCase(IMediator mediator) : base(mediator)
        { 
        }

        public async Task<bool> Executar(long devolutivaId)
        {
            await mediator.Send(new ExcluirReferenciaDiarioBordoDevolutivaCommand(devolutivaId));

            await mediator.Send(new ExcluirDevolutivaCommand(devolutivaId));

            return true;
        }
    }
}
