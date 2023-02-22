using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasDevolutivaPorTurmaEComponenteCommandHandler : IRequestHandler<ExcluirPendenciasDevolutivaPorTurmaEComponenteCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaDevolutiva _repositorioPendenciaDevolutiva;

        public ExcluirPendenciasDevolutivaPorTurmaEComponenteCommandHandler(IMediator mediator,
            IRepositorioPendenciaDevolutiva repositorioPendenciaDevolutiva)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this._repositorioPendenciaDevolutiva = repositorioPendenciaDevolutiva ?? throw new ArgumentNullException(nameof(repositorioPendenciaDevolutiva));
        }

        public async Task<bool> Handle(ExcluirPendenciasDevolutivaPorTurmaEComponenteCommand request, CancellationToken cancellationToken)
        {
            var existePendencias = await _repositorioPendenciaDevolutiva.ExistePendenciasDevolutivaPorTurmaComponente(request.TurmaId, request.ComponenteId);

            if (!existePendencias)
                return await Task.FromResult(false);

            var existePendenciaDiarioBordo = await mediator.Send(new ExistePendenciaDiarioBordoQuery(request.TurmaId, request.ComponenteId.ToString()), cancellationToken);

            if (!existePendenciaDiarioBordo)
            {
                await _repositorioPendenciaDevolutiva.ExlusaoLogicaPorTurmaComponente(request.TurmaId, request.ComponenteId);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
