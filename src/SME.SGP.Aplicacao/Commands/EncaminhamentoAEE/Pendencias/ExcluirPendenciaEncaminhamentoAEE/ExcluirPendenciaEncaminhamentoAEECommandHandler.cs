using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaEncaminhamentoAEECommandHandler : IRequestHandler<ExcluirPendenciaEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;

        public ExcluirPendenciaEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new System.ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
        }

        public async Task<bool> Handle(ExcluirPendenciaEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaEncaminhamentoAEE.Excluir(request.PendenciaId);
            await mediator.Send(new ExcluirPendenciasUsuariosPorPendenciaIdCommand(request.PendenciaId));
            await mediator.Send(new ExcluirPendenciaPorIdCommand(request.PendenciaId));

            return true;

        }
    }
}
