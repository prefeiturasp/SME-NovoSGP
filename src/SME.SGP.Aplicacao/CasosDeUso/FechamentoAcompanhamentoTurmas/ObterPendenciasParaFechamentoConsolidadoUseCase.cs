using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParaFechamentoConsolidadoUseCase : AbstractUseCase, IObterPendenciasParaFechamentoConsolidadoUseCase
    {
        public ObterPendenciasParaFechamentoConsolidadoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PendenciaParaFechamentoConsolidadoDto>> Executar(FiltroPendenciasFechamentoConsolidadoDto param)
        {
            var pendenciasFechamento = await mediator.Send(new ObterPendenciasParaFechamentoConsolidadoQuery(param.TurmaId, param.Bimestre, param.ComponenteCurricularId));
            return pendenciasFechamento;
        }
    }
}
