using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReplicarParametrosAnoAnteriorCommandHandler : IRequestHandler<ReplicarParametrosAnoAnteriorCommand, bool>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IMediator mediator;

        public ReplicarParametrosAnoAnteriorCommandHandler(IRepositorioParametrosSistema repositorioParametrosSistema,
                                                           IMediator mediator)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ReplicarParametrosAnoAnteriorCommand request, CancellationToken cancellationToken)
        {
            var anoLetivoAnterior = await mediator
                .Send(new ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery(request.AnoLetivo, request.ModalidadeTipoCalendario));
            
            await repositorioParametrosSistema
                .ReplicarParametrosAnoAnteriorAsync(request.AnoLetivo, anoLetivoAnterior);

            return true;
        }
    }
}
