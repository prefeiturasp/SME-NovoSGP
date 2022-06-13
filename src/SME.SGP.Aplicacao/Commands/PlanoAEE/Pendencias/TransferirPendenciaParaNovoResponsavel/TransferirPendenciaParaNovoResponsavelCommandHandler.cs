using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TransferirPendenciaParaNovoResponsavelCommandHandler : IRequestHandler<TransferirPendenciaParaNovoResponsavelCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaPlanoAEE repositorioPendenciaPlano;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;

        public TransferirPendenciaParaNovoResponsavelCommandHandler(
                                                IMediator mediator, 
                                                IRepositorioPendenciaPlanoAEE repositorioPendenciaPlano, 
                                                IRepositorioPendenciaUsuario repositorioPendenciaUsuario)
        {
            this.mediator = mediator;
            this.repositorioPendenciaPlano = repositorioPendenciaPlano;
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario;
        }

        public async Task<bool> Handle(TransferirPendenciaParaNovoResponsavelCommand request, CancellationToken cancellationToken)
        {
            var listaDePendenciaPlano = await repositorioPendenciaPlano.ObterPorPlanoId(request.PlanoAeeId);

            foreach (var pendenciaPlano in listaDePendenciaPlano)
                await repositorioPendenciaUsuario.AlteraUsuarioDaPendencia(pendenciaPlano.PendenciaId, request.ResponsavelId);

            return true;
        }
    }
}
