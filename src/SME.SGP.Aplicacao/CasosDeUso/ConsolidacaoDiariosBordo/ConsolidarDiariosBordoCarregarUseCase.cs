using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarDiariosBordoCarregarUseCase : AbstractUseCase, IConsolidarDiariosBordoCarregarUseCase
    {
        private ParametrosSistema parametroConsolidacao;

        public ConsolidarDiariosBordoCarregarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var anoLetivo = DateTime.Now.Year;

            if (!await ExecutarConsolidacao(anoLetivo)) 
                return true;

            await LimparConsolidacoes(anoLetivo);

            var ues = await mediator.Send(new ObterTodasUesIdsQuery());
            foreach (var ue in ues)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDiariosBordoPorUeTratar, new FiltroConsolidacaoDiariosBordoPorUeDto(ue), new System.Guid(), null));

            await AtualizarDataExecucao();

            return true;
        }

        private async Task LimparConsolidacoes(int anoLetivo)
        {
            await mediator.Send(new RemoverConsolidacoesDiariosBordoCommand(anoLetivo));
        }

        private async Task AtualizarDataExecucao()
        {
            parametroConsolidacao.Valor = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            await mediator.Send(new AtualizarParametroSistemaCommand(parametroConsolidacao));
        }

        private async Task<bool> ExecutarConsolidacao(int anoLetivo)
        {
            parametroConsolidacao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.ExecucaoConsolidacaoDiariosBordo, anoLetivo));
            return parametroConsolidacao?.Ativo ?? false;
        }
    }
}
