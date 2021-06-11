using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAEEObservacaoUseCase : AbstractUseCase, IExcluirPlanoAEEObservacaoUseCase
    {
        public ExcluirPlanoAEEObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long id)
        {
            return await mediator.Send(new ExcluirPlanoAEEObservacaoCommand(id));
        }
    }
}
