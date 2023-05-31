using System;
using System.Linq;
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

            if (!parametroEncaminhamento && !parametroAtendimento)
                throw new NegocioException("Nenhum parâmetro ativo no banco, os parâmetros de encaminhamento e atendimento precisam estar ativos");

            foreach (var ueId in listaUes)
            {
                if (parametroEncaminhamento)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarBuscarUesConsolidadoEncaminhamentoNAAPA, new FiltroBuscarUesConsolidadoEncaminhamentoNAAPADto(ueId, anoLetivo), Guid.NewGuid()));
                if (parametroAtendimento)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPA, new FiltroBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto(ueId, DateTimeExtension.HorarioBrasilia().Month, DateTimeExtension.HorarioBrasilia().Year), Guid.NewGuid()));
            }

            await AtualizarDataExecucao(parametroEncaminhamento, TipoParametroSistema.GerarConsolidadoEncaminhamentoNAAPA);
            await AtualizarDataExecucao(parametroAtendimento, TipoParametroSistema.GerarConsolidadoAtendimentoNAAPA);
            
            return true;
        }

        private async Task AtualizarDataExecucao(bool executar, TipoParametroSistema tipo)
        {
            if (executar)
            {
                var tipos = new long[] { (long)tipo };
                var parametroSistema = (await mediator.Send(new ObterParametrosSistemaPorTiposQuery() { Tipos = tipos })).FirstOrDefault();
                if (parametroSistema != null)
                {
                    parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");
                    await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
                }
            }
        }
    }
}