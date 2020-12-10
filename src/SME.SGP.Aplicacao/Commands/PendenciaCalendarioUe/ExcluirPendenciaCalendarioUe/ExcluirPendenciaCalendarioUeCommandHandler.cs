using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PendenciaCalendarioUe.ExcluirPendenciaCalendarioUe
{
    public class ExcluirPendenciaCalendarioUeCommandHandler : IRequestHandler<ExcluirPendenciaCalendarioUeCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe;

        public ExcluirPendenciaCalendarioUeCommandHandler(IMediator mediator, IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaCalendarioUe = repositorioPendenciaCalendarioUe ?? throw new ArgumentNullException(nameof(repositorioPendenciaCalendarioUe));
        }

        public async Task<bool> Handle(ExcluirPendenciaCalendarioUeCommand request, CancellationToken cancellationToken)
        {
            var pendenciasCalendario = await repositorioPendenciaCalendarioUe.ObterPendenciasPorCalendarioUe(request.TipoCalendarioId, request.UeId, request.TipoPendencia);
            foreach(var pendenciaCalendario in pendenciasCalendario)
            {
                repositorioPendenciaCalendarioUe.Remover(pendenciaCalendario);

                await mediator.Send(new ExcluirPendenciaPorIdCommand(pendenciaCalendario.PendenciaId));
            }

            return true;
        }
    }
}
