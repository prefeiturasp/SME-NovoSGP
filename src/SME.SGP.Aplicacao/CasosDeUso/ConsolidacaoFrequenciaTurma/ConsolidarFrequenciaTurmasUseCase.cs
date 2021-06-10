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

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoFrequenciaTurma, ano));
            if (parametroSistema != null)
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");

                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
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
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarFrequenciasTurmasNoAno, new FiltroAnoDto(ano), Guid.NewGuid(), null));
            await AtualizarDataExecucao(ano);
        }
    }
}
