using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDetalhamentoPendenciaAulaQueryHandler : IRequestHandler<ObterDetalhamentoPendenciaAulaQuery, DetalhamentoPendenciaAulaDto>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ObterDetalhamentoPendenciaAulaQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<DetalhamentoPendenciaAulaDto> Handle(ObterDetalhamentoPendenciaAulaQuery request, CancellationToken cancellationToken)
        {
            var detalhamentoPendenciaAula = await repositorioPendenciaFechamento.ObterDetalhamentoPendenciaAula(request.PendenciaId);
            return detalhamentoPendenciaAula;
        }
    }
}
