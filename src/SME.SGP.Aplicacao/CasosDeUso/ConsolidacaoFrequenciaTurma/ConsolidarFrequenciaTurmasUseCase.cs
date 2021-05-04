using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
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
                var anoAtual = DateTime.Now.Year;

                await ConsolidarFrequenciaTurmasHistorico(anoAtual);

                await ConsolidarFrequenciaTurmasAnoAtual(anoAtual);

                return true;

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }        
        }

        private async Task ConsolidarFrequenciaTurmasAnoAtual(int anoAtual)
        {
            await mediator.Send(new LimparConsolidacaoFrequenciaTurmasPorAnoCommand(anoAtual));
            await ConsolidarFrequenciasTurmasNoAno(anoAtual);
        }

        private async Task ConsolidarFrequenciaTurmasHistorico(int anoAtual)
        {
            for (var ano = 2014; ano < anoAtual; ano++)
            {
                if (!await mediator.Send(new ExisteConsolidacaoFrequenciaTurmaPorAnoQuery(ano)))
                    await ConsolidarFrequenciasTurmasNoAno(ano);
            }
        }

        private async Task ConsolidarFrequenciasTurmasNoAno(int ano)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.ConsolidarFrequenciasTurmasNoAno, new FiltroAnoDto(ano), Guid.NewGuid(), null, fila: RotasRabbit.ConsolidarFrequenciasTurmasNoAno));
        }
    }
}
