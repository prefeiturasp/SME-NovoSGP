using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQueryHandler : IRequestHandler<ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaFechamentoAtividadeAvaliativaConsulta repositorioPendenciaFechamentoAtividadeAvaliativaConsulta;

        public ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQueryHandler(IRepositorioPendenciaFechamentoAtividadeAvaliativaConsulta repositorioPendenciaFechamentoAtividadeAvaliativaConsulta)
        {
            this.repositorioPendenciaFechamentoAtividadeAvaliativaConsulta = repositorioPendenciaFechamentoAtividadeAvaliativaConsulta ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamentoAtividadeAvaliativaConsulta));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsAtividadeAvaliativaDaPendenciaDeFechamentoQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioPendenciaFechamentoAtividadeAvaliativaConsulta.ObterIdsAtividadeAvaliativaDaPendenciaDeFechamento(request.IdsPendenciaFechamento);
        }
    }
}
