using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase : AbstractUseCase,IExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase
    {
        public ExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoLetivo = !string.IsNullOrEmpty(param.Mensagem?.ToString()) ? int.Parse(param.Mensagem.ToString()!) : DateTime.Now.Year;
            var listaUes = await mediator.Send(new ObterTodasUesIdsQuery());

            foreach (var ueId in listaUes.Where(ue => ue == 1))
            {
                //await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarBuscarUesConsolidadoEncaminhamentoNAAPA, new FiltroBuscarUesConsolidadoEncaminhamentoNAAPADto(ueId, anoLetivo), Guid.NewGuid()));
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPA, new FiltroBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto(ueId, DateTime.Now.Month, DateTime.Now.Year), Guid.NewGuid()));
            }

            return true;
        }
    }
}