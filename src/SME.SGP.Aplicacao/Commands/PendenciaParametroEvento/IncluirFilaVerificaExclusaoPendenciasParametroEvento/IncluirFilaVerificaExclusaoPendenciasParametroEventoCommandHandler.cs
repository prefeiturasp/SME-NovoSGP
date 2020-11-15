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
    public class IncluirFilaVerificaExclusaoPendenciasParametroEventoCommandHandler : IRequestHandler<IncluirFilaVerificaExclusaoPendenciasParametroEventoCommand, bool>
    {
        private readonly IServicoFila servicoFila;

        public IncluirFilaVerificaExclusaoPendenciasParametroEventoCommandHandler(IServicoFila servicoFila)
        {
            this.servicoFila = servicoFila ?? throw new ArgumentNullException(nameof(servicoFila));
        }

        public Task<bool> Handle(IncluirFilaVerificaExclusaoPendenciasParametroEventoCommand request, CancellationToken cancellationToken)
        {
            servicoFila.PublicaFilaWorkerSgp(new PublicaFilaSgpDto(RotasRabbit.RotaExecutaExclusaoPendenciaParametroEvento,
                                                       new VerificaExclusaoPendenciasParametroEventoCommand(request.TipoCalendarioId, request.UeCodigo, request.TipoEvento),
                                                       Guid.NewGuid(),
                                                       request.Usuario));

            return Task.FromResult(true);
        }
    }
}
