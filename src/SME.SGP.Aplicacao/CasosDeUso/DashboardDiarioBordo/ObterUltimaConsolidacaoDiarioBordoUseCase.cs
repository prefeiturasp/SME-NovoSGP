using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaConsolidacaoDiarioBordoUseCase : AbstractUseCase, IObterUltimaConsolidacaoDiarioBordoUseCase
    {
        public ObterUltimaConsolidacaoDiarioBordoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<string> Executar(int anoLetivo)
        {
            var parametroConsolidacao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.ExecucaoConsolidacaoDiariosBordo, anoLetivo));
            return parametroConsolidacao?.Valor ?? "";
        }
    }
}
