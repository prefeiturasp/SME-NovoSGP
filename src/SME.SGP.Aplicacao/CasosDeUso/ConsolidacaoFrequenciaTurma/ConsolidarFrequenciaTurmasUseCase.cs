using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmasUseCase : AbstractUseCase, IConsolidarFrequenciaTurmasUseCase
    {
        public ConsolidarFrequenciaTurmasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {

                if (!await ExecutarConsolidacaoFrequencia())
                    return false;

                await ConsolidarFrequenciaTurmasHistorico();

                await ConsolidarFrequenciaTurmasAnoAtual();

                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private async Task<bool> ExecutarConsolidacaoFrequencia()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoFrequenciaTurma, DateTime.Now.Year));
            if (parametroExecucao != null)
                return parametroExecucao.Ativo;

            return false;
        }

        private async Task ConsolidarFrequenciaTurmasAnoAtual()
        {
            var anoAtual = DateTime.Now.Year;
            await mediator.Send(new LimparConsolidacaoFrequenciaTurmasPorAnoCommand(anoAtual));
            await ConsolidarFrequenciasTurmasNoAno(anoAtual);
        }

        private async Task ConsolidarFrequenciaTurmasHistorico()
        {
            for (var ano = 2014; ano < DateTime.Now.Year; ano++)
            {
                if (!await mediator.Send(new ExisteConsolidacaoFrequenciaTurmaPorAnoQuery(ano)))
                {
                    await ConsolidarFrequenciasTurmasNoAno(ano);
                }
            }
        }

        private async Task ConsolidarFrequenciasTurmasNoAno(int ano)
        {
            await mediator.Send(new ExecutarConsolidacaoFrequenciaNoAnoCommand(ano));
        }
    }
}
