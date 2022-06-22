using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelGeralDoPlanoCommandHandler : IRequestHandler<AtribuirResponsavelGeralDoPlanoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public AtribuirResponsavelGeralDoPlanoCommandHandler(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<bool> Handle(AtribuirResponsavelGeralDoPlanoCommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException("O Plano AEE informado não foi encontrado");

            planoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(request.ResponsavelRF));

            await repositorioPlanoAEE.SalvarAsync(planoAEE);

            await TransfereResponsavelDaPendecia(planoAEE);

            return true;
        }

        private async Task TransfereResponsavelDaPendecia(PlanoAEE plano)
        {
            var command = new TransferirPendenciaParaNovoResponsavelCommand(plano.Id, plano.ResponsavelId);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTransferirPendenciaPlanoAEEParaNovoResponsavel, command, Guid.NewGuid()));
        }
    }
}
