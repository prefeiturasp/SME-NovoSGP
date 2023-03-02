using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

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
