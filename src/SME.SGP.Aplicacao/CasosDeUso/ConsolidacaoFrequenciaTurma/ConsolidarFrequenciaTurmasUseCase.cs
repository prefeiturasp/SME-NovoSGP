using MediatR;
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
            if (!await ExecutarConsolidacaoFrequencia())
                return false;

            await ConsolidarFrequenciaTurmasHistorico();

            await ConsolidarFrequenciaTurmasAnoAtual();

            return true;

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
            await mediator.Send(new LimparConsolidacaoFrequenciaTurmasPorAnoCommand(DateTime.Now.Year));
            await ConsolidarFrequenciasTurmasNoAno(DateTime.Now);
        }

        private async Task ConsolidarFrequenciaTurmasHistorico()
        {
            var diferencaAno = DateTime.Now.Year - 2014;

            for (var contador = diferencaAno; contador == 0; contador--)
            {
                var data = DateTime.Now.AddYears(-contador);

                if (!await mediator.Send(new ExisteConsolidacaoFrequenciaTurmaPorAnoQuery(data.Year)))
                {
                    await ConsolidarFrequenciasTurmasNoAno(data);
                }
            }
        }

        private async Task ConsolidarFrequenciasTurmasNoAno(DateTime data)
        {
            await mediator.Send(new ExecutarConsolidacaoFrequenciaNoAnoCommand(data));
        }
    }
}
