using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TransferirPendenciaParaNovoResponsavelCommandHandler : IRequestHandler<TransferirPendenciaParaNovoResponsavelCommand, bool>
    {
        private readonly IRepositorioPendenciaPlanoAEE repositorioPendenciaPlano;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;

        public TransferirPendenciaParaNovoResponsavelCommandHandler(IRepositorioPendenciaPlanoAEE repositorioPendenciaPlano, 
                                                IRepositorioPendenciaUsuario repositorioPendenciaUsuario)
        {
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
