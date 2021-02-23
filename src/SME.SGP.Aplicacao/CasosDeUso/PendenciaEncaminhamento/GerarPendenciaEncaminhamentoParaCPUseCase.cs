using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaEncaminhamentoAEEParaCPUseCase : AbstractUseCase, IGerarPendenciaEncaminhamentoAEEParaCPUseCase
    {
        public GerarPendenciaEncaminhamentoAEEParaCPUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
        }
    }
}
