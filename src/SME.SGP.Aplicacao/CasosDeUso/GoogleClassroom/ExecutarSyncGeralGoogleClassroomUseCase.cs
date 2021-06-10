using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSyncGeralGoogleClassroomUseCase : IExecutarSyncGeralGoogleClassroomUseCase
    {
        protected readonly IMediator mediator;

        public ExecutarSyncGeralGoogleClassroomUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar()
        {
            string mensagem = "Mensagem API Google Classroom";
            return await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbitSgpGoogleClassroomApi.FilaGoogleSync, mensagem));
        }
    }
}
