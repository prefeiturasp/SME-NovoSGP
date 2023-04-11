using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarLogUseCase : ISalvarLogUseCase
    {
        private readonly IMediator mediator;

        public SalvarLogUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public Task SalvarInformacao(string mensagem, LogContexto logContexto)
            => mediator.Send(new SalvarLogViaRabbitCommand(mensagem, LogNivel.Informacao, logContexto));
    }
}
