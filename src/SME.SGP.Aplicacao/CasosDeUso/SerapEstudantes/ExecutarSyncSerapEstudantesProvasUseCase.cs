using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSyncSerapEstudantesProvasUseCase : IExecutarSyncSerapEstudantesProvasUseCase
    {
        protected readonly IMediator mediator;

        public ExecutarSyncSerapEstudantesProvasUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            return await mediator.Send(new PublicarFilaSerapEstudantesCommand(RotasRabbitSerapEstudantes.FilaProvaSync, string.Empty));
        }
    }
}
