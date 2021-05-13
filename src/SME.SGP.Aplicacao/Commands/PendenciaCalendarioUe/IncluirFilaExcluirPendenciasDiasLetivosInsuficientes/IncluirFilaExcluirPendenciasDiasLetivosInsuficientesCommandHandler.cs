using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommandHandler : IRequestHandler<IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand request, CancellationToken cancellationToken)
        {
            return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes,
                                                       new ExcluirPendenciasDiasLetivosInsuficientesCommand(request.TipoCalendarioId, request.DreCodigo, request.UeCodigo),
                                                       Guid.NewGuid(),
                                                       request.Usuario));
        }
    }
}
