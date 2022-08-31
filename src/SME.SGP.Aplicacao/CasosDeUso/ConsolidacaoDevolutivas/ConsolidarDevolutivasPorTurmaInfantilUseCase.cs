using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ConsolidarDevolutivasPorTurmaInfantilUseCase : AbstractUseCase, IConsolidarDevolutivasPorTurmaInfantilUseCase
    {
        public ConsolidarDevolutivasPorTurmaInfantilUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {

            if (!await ExecutarConsolidacaoDevolutivas())
                return false;

            await ConsolidarDevolutivasAnoAtual();

            await ConsolidarDevolutivasHistorico();

            return true;

        }

        private async Task ConsolidarDevolutivasAnoAtual()
        {
            try
            {
                var anoAtual = DateTime.Now.Year;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDevolutivasPorTurmaInfantilTurma, new FiltroCodigoTurmaInfantilPorAnoDto(anoAtual), Guid.NewGuid(), null));
            }
            catch (Exception ex)
            {

                await mediator.Send(new SalvarLogViaRabbitCommand($"Consolidar Devolutivas Por Turmas Infantil", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
            }
        }

        private async Task ConsolidarDevolutivas(int ano)
        {
            try
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDevolutivasPorTurmaInfantilTurma, new FiltroCodigoTurmaInfantilPorAnoDto(ano), Guid.NewGuid(), null));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Erro ao executar", LogNivel.Critico, LogContexto.Geral, ex.Message));
            }
        }

        private async Task ConsolidarDevolutivasHistorico()
        {
            for (var ano = 2021; ano < DateTime.Now.Year; ano++)
            {
                if (!await mediator.Send(new ExisteConsolidacaoDevolutivaTurmaPorAnoQuery(ano)))
                {
                    await ConsolidarDevolutivas(ano);
                }
            }
        }

        private async Task<bool> ExecutarConsolidacaoDevolutivas()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, DateTime.Now.Year));
            if (parametroExecucao != null)
                return parametroExecucao.Ativo;

            return false;
        }
    }
}