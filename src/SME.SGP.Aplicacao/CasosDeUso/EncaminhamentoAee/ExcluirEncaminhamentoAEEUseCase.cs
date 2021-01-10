using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirEncaminhamentoAEEUseCase : AbstractUseCase, IExcluirEncaminhamentoAEEUseCase
    {
        public ExcluirEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoAeeId)
            => await mediator.Send(new ExcluirEncaminhamentoAEECommand(encaminhamentoAeeId));
    }
}
