using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaControllerUseCase : AbstractUseCase, IExecutaConsolidacaoDiariaDashBoardFrequenciaControllerUseCase
    {
        public ExecutaConsolidacaoDiariaDashBoardFrequenciaControllerUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroConsolicacaoDiariaDashBoardFrequenciaDto filtroConsolicacao)
        {
            if (filtroConsolicacao == null)
                throw new NegocioException(MensagemNegocioComuns.NAO_FOI_POSSIVEL_INICIAR_A_CONSOLIDACAO_DIARIA);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequencia, filtroConsolicacao, Guid.NewGuid(), null));

            return true;
        }
    }
}
