using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsAulaDaPendenciaDeFechamentoQueryHandler : IRequestHandler<ObterIdsAulaDaPendenciaDeFechamentoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaFechamentoAulaConsulta repositorioPendenciaFechamentoAulaConsulta;

        public ObterIdsAulaDaPendenciaDeFechamentoQueryHandler(IRepositorioPendenciaFechamentoAulaConsulta repositorioPendenciaFechamentoAulaConsulta)
        {
            this.repositorioPendenciaFechamentoAulaConsulta = repositorioPendenciaFechamentoAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamentoAulaConsulta));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsAulaDaPendenciaDeFechamentoQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioPendenciaFechamentoAulaConsulta.ObterIdsAulaDaPendenciaDeFechamento(request.IdsPendenciaFechamento);
        }
    }
}
