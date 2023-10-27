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
        private readonly IServicoUsuario servicoUsuario;

        public AtribuirResponsavelGeralDoPlanoCommandHandler(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE, IServicoUsuario servicoUsuario)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<bool> Handle(AtribuirResponsavelGeralDoPlanoCommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoAEEId));

            if (planoAEE.EhNulo())
                throw new NegocioException("O Plano AEE informado não foi encontrado");

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(request.ResponsavelRF, String.Empty, request.ResponsavelNome);

            planoAEE.ResponsavelId = usuario.Id;

            await repositorioPlanoAEE.SalvarAsync(planoAEE);

            await TransfereResponsavelDaPendecia(planoAEE);

            return true;
        }

        private async Task TransfereResponsavelDaPendecia(PlanoAEE plano)
        {
            var command = new TransferirPendenciaParaNovoResponsavelCommand(plano.Id, plano.ResponsavelId);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.RotaTransferirPendenciaPlanoAEEParaNovoResponsavel, command, Guid.NewGuid()));
        }
    }
}
