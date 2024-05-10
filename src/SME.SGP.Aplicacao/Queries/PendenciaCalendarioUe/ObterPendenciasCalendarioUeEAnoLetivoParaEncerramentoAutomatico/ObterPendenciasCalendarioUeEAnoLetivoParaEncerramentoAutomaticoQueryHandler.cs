using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQueryHandler : IRequestHandler<ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQuery, IEnumerable<long>>
    {
        public ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQueryHandler(IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe)
        {
            this.repositorioPendenciaCalendarioUe = repositorioPendenciaCalendarioUe ?? throw new ArgumentNullException(nameof(repositorioPendenciaCalendarioUe));
        }

        private readonly IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe;


        public async Task<IEnumerable<long>> Handle(ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaCalendarioUe.ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomatico(request.UeId, request.AnoLetivo);
        }
    }
}