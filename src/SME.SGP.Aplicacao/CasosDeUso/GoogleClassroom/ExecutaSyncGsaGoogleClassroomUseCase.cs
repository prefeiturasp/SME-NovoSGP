using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaSyncGsaGoogleClassroomUseCase : AbstractUseCase, IExecutaSyncGsaGoogleClassroomUseCase
    {
        public ExecutaSyncGsaGoogleClassroomUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar()
        {
            string mensagem = "Mensagem API Google Classroom - Comparativo de Dados";
            return await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbitGoogleClassroomApi.FilaGsaSync, mensagem));
        }
    }
}