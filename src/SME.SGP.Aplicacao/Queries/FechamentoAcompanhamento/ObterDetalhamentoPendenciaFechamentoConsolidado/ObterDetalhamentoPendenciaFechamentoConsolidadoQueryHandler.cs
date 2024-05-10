using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDetalhamentoPendenciaFechamentoConsolidadoQueryHandler : IRequestHandler<ObterDetalhamentoPendenciaFechamentoConsolidadoQuery, DetalhamentoPendenciaFechamentoConsolidadoDto>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ObterDetalhamentoPendenciaFechamentoConsolidadoQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<DetalhamentoPendenciaFechamentoConsolidadoDto> Handle(ObterDetalhamentoPendenciaFechamentoConsolidadoQuery request, CancellationToken cancellationToken)
        {
            var pendenciaFechamento = await repositorioPendenciaFechamento.ObterDetalhamentoPendenciaFechamentoConsolidado(request.PendenciaId);
            return pendenciaFechamento;
        }
    }
}
