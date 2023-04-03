using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPendenciaFechamentoAprovadaResolvidaQueryHandler : IRequestHandler<ObterIdPendenciaFechamentoAprovadaResolvidaQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ObterIdPendenciaFechamentoAprovadaResolvidaQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdPendenciaFechamentoAprovadaResolvidaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaFechamento.ObterIdPendenciaFechamentoAprovadaResolvida(request.FechamentoId, request.TipoPendencia);
        }
    }
}
