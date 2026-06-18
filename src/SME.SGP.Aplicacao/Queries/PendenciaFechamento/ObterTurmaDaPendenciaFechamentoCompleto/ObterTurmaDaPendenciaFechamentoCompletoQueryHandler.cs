using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaFechamentoCompletoQueryHandler : IRequestHandler<ObterTurmaDaPendenciaFechamentoCompletoQuery, PendenciaFechamentoCompletoDto>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ObterTurmaDaPendenciaFechamentoCompletoQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<PendenciaFechamentoCompletoDto> Handle(ObterTurmaDaPendenciaFechamentoCompletoQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaFechamento.ObterPorPendenciaId(request.PendenciaId);
    }
}
