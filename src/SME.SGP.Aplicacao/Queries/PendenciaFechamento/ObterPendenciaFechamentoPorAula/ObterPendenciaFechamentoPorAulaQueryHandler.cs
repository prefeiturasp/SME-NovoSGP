using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaFechamentoPorAulaQueryHandler : IRequestHandler<ObterPendenciaFechamentoPorAulaQuery, IEnumerable<PendenciaFechamento>>
    {
        private readonly IRepositorioPendenciaFechamentoAulaConsulta repositorioPendenciaFechamentoAulaConsulta;

        public ObterPendenciaFechamentoPorAulaQueryHandler(IRepositorioPendenciaFechamentoAulaConsulta repositorioPendenciaFechamentoAulaConsulta)
        {
            this.repositorioPendenciaFechamentoAulaConsulta = repositorioPendenciaFechamentoAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamentoAulaConsulta));
        }

        public async Task<IEnumerable<PendenciaFechamento>> Handle(ObterPendenciaFechamentoPorAulaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaFechamentoAulaConsulta.ObterPendenciaFechamentoDeAula(request.IdAula, request.TipoPendencia);
        }
    }
}
