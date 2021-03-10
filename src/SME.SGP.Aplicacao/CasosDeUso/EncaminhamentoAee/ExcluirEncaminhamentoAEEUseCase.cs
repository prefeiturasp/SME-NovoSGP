using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirEncaminhamentoAEEUseCase : AbstractUseCase, IExcluirEncaminhamentoAEEUseCase
    {
        public ExcluirEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoAeeId)
        {
            await mediator.Send(new ExcluirEncaminhamentoAEECommand(encaminhamentoAeeId));

            await ExcluirPendenciasEncaminhamentoAEE(encaminhamentoAeeId);

            return true;
        }

        private async Task ExcluirPendenciasEncaminhamentoAEE(long encaminhamentoId)
        {
            var pendenciasEncaminhamentoAEE = await mediator.Send(new ObterPendenciasDoEncaminhamentoAEEPorIdQuery(encaminhamentoId));
            if (pendenciasEncaminhamentoAEE != null || !pendenciasEncaminhamentoAEE.Any())
            {
                foreach (var pendenciaEncaminhamentoAEE in pendenciasEncaminhamentoAEE)
                {
                    await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendenciaEncaminhamentoAEE.PendenciaId));
                }
            }
        }
    }
}
