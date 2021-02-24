using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoGoogleClassroomUseCase : IExecutaNotificacaoGoogleClassroomUseCase
    {
        protected readonly IMediator mediator;

        public ExecutaNotificacaoGoogleClassroomUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar()
        {
            string mensagem = "Mensagem API Google Classroom";
            return await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbit.FilaGoogleSync, mensagem));
        }
    }
}
