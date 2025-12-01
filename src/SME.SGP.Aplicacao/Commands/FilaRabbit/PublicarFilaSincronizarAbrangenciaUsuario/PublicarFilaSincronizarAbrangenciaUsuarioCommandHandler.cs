using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Mensageria.Exchange;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.FilaRabbit.PublicarFilaSincronizarAbrangenciaUsuario
{
    public class PublicarFilaSincronizarAbrangenciaUsuarioCommandHandler : IRequestHandler<PublicarFilaSincronizarAbrangenciaUsuarioCommand, bool>
    {
        private readonly IServicoMensageriaSGP servicoMensageria;
        private readonly IMediator mediator;

        public PublicarFilaSincronizarAbrangenciaUsuarioCommandHandler(IServicoMensageriaSGP servicoMensageria, IMediator mediator)
        {
            this.servicoMensageria = servicoMensageria;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(PublicarFilaSincronizarAbrangenciaUsuarioCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.WorkerAbrangencia, command, new System.Guid(), null, false, ExchangeWorkerAbrangenciaRabbit.WorkerAbrangencia));
            return true;
        }
    }
}
