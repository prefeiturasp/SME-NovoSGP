using MediatR;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasEncaminhamentoAEECPCommandHandler : IRequestHandler<ExcluirPendenciasEncaminhamentoAEECPCommand, bool>
    {
        private readonly IMediator mediator;

        public ExcluirPendenciasEncaminhamentoAEECPCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirPendenciasEncaminhamentoAEECPCommand request, CancellationToken cancellationToken)
        {
            return await RemoverPendenciasCP(request.TurmaId, request.EncaminhamentoId);
        }

        public async Task<bool> RemoverPendenciasCP(long turmaId, long encaminhamentoAEEId)
        {
            var ue = await mediator.Send(new ObterUEPorTurmaIdQuery(turmaId));
            if (ue.EhNulo())
                return false;

            var pendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdQuery(encaminhamentoAEEId));
            if (pendencia.NaoEhNulo())
                await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendencia.PendenciaId));

            return true;
        }
    }
}
