using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase : AbstractUseCase, IExecutarCargaConsolidadoAtendimentoNAAPAUseCase
    {
        public ExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoLetivo = !string.IsNullOrEmpty(param.Mensagem?.ToString()) ? int.Parse(param.Mensagem.ToString()!) : DateTimeExtension.HorarioBrasilia().Year;
            var ConsolidarEncaminhamento = await ObterParametro(TipoParametroSistema.GerarConsolidadoEncaminhamentoNAAPA, anoLetivo);
            var ConsolidarAtendimento = await ObterParametro(TipoParametroSistema.GerarConsolidadoAtendimentoNAAPA, anoLetivo);

            if ((ConsolidarEncaminhamento?.Ativo != true)
                && (ConsolidarAtendimento?.Ativo != true))
                throw new NegocioException("Nenhum parâmetro ativo no banco, os parâmetros de encaminhamento e atendimento precisam estar ativos");

            var listaUes = await mediator.Send(ObterTodasUesIdsQuery.Instance);
            foreach (var ueId in listaUes)
            {
                if (ConsolidarEncaminhamento?.Ativo == true)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarBuscarUesConsolidadoEncaminhamentoNAAPA, new FiltroBuscarUesConsolidadoAtendimentoNAAPADto(ueId, anoLetivo), Guid.NewGuid()));
                if (ConsolidarAtendimento?.Ativo == true)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPA, new FiltroBuscarAtendimentosProfissionalConsolidadoAtendimentoNAAPADto(ueId, DateTimeExtension.HorarioBrasilia().Month, DateTimeExtension.HorarioBrasilia().Year), Guid.NewGuid()));
            }

            await AtualizarDataExecucao(ConsolidarEncaminhamento);
            await AtualizarDataExecucao(ConsolidarAtendimento);
            
            return true;
        }

        private Task<ParametrosSistema> ObterParametro(TipoParametroSistema tipoParametro, int anoLetivo)
            => mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(tipoParametro, anoLetivo));

        private async Task AtualizarDataExecucao(ParametrosSistema parametro)
        {
            if (parametro?.Ativo == true)
            {
                parametro.Valor = DateTimeExtension.HorarioBrasilia().ToString("yyyy-MM-dd HH:mm:ss.fff tt");
                await mediator.Send(new AtualizarParametroSistemaCommand(parametro));
            }
        }
    }
}