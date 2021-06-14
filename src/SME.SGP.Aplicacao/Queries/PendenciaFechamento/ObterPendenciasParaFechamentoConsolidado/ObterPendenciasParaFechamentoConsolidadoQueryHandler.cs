using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParaFechamentoConsolidadoQueryHandler : IRequestHandler<ObterPendenciasParaFechamentoConsolidadoQuery, IEnumerable<PendenciaParaFechamentoConsolidadoDto>>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ObterPendenciasParaFechamentoConsolidadoQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<IEnumerable<PendenciaParaFechamentoConsolidadoDto>> Handle(ObterPendenciasParaFechamentoConsolidadoQuery request, CancellationToken cancellationToken)
        {
            var pendenciasFechamento = await repositorioPendenciaFechamento.ObterPendenciasParaFechamentoConsolidado(request.TurmaId, request.Bimestre, request.ComponenteCurricularId);
            return pendenciasFechamento;
        }
    }
}
