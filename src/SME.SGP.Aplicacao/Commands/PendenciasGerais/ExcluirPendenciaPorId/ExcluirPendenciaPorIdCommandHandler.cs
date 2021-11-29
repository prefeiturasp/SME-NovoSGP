using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaPorIdCommandHandler : IRequestHandler<ExcluirPendenciaPorIdCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendencia repositorioPendencia;

        public ExcluirPendenciaPorIdCommandHandler(IMediator mediator, IRepositorioPendencia repositorioPendencia)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<bool> Handle(ExcluirPendenciaPorIdCommand request, CancellationToken cancellationToken)
        {
            var pendencia = await repositorioPendencia.ObterPorIdAsync(request.PendenciaId);
            pendencia.Excluido = true;
            await repositorioPendencia.SalvarAsync(pendencia);

            var pendenciaPerfilId = await mediator.Send(new ObterPendenciaPerfilPorPendenciaIdQuery(pendencia.Id)); 

            foreach(var pendencias in pendenciaPerfilId)
            {
                await mediator.Send(new ExcluirPendenciasUsuariosPorPendenciaIdCommand(pendencias.Id));
            }

            return true;
        }
    }
}
