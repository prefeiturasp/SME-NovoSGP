using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommandHandler : IRequestHandler<IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand, bool>
    {
        private readonly IServicoFila servicoFila;

        public IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommandHandler(IServicoFila servicoFila)
        {
            this.servicoFila = servicoFila ?? throw new ArgumentNullException(nameof(servicoFila));
        }

        public Task<bool> Handle(IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand request, CancellationToken cancellationToken)
        {
            servicoFila.PublicaFilaWorkerSgp(new PublicaFilaSgpDto(RotasRabbit.RotaExecutaExclusaoPendenciaParametroEvento,
                                                       new ExcluirPendenciasDiasLetivosInsuficientesCommand(request.TipoCalendarioId, request.DreCodigo, request.UeCodigo),
                                                       Guid.NewGuid(),
                                                       request.Usuario));

            return Task.FromResult(true);
        }
    }
}
