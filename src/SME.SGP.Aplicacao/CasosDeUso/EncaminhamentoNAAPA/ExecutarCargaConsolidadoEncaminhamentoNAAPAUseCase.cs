using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase : AbstractUseCase, IExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase
    {
        public ExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoLetivo = !string.IsNullOrEmpty(param.Mensagem?.ToString()) ? int.Parse(param.Mensagem.ToString()!) : DateTimeExtension.HorarioBrasilia().Year;
            var listaUes = await mediator.Send(new ObterTodasUesIdsQuery());
            var parametroEncaminhamento = await mediator.Send(new VerificaSeExisteParametroSistemaPorTipoQuery(TipoParametroSistema.GerarConsolidadoEncaminhamentoNAAPA));
            var parametroAtendimento = await mediator.Send(new VerificaSeExisteParametroSistemaPorTipoQuery(TipoParametroSistema.GerarConsolidadoAtendimentoNAAPA));

            foreach (var ueId in listaUes)
            {
                if (parametroEncaminhamento)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarBuscarUesConsolidadoEncaminhamentoNAAPA, new FiltroBuscarUesConsolidadoEncaminhamentoNAAPADto(ueId, anoLetivo), Guid.NewGuid()));
                if (parametroAtendimento)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPA, new FiltroBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto(ueId, DateTimeExtension.HorarioBrasilia().Month, DateTimeExtension.HorarioBrasilia().Year), Guid.NewGuid()));
            }

            return true;
        }
    }
}