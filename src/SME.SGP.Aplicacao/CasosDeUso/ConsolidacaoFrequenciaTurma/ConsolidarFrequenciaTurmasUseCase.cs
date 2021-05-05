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
        private ParametrosSistema parametroExecucao;

        public ConsolidarFrequenciaTurmasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var anoAtual = DateTime.Now.Year;

                if (!await ExecutarConsolidacaoFrequencia(anoAtual))
                    return false;

                await ConsolidarFrequenciaTurmasHistorico(anoAtual);

                await ConsolidarFrequenciaTurmasAnoAtual(anoAtual);

                await AtualizarDataExecucao(anoAtual);

                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }        
        }

        private async Task<bool> ExecutarConsolidacaoFrequencia(int anoAtual)
        {
            parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoFrequenciaTurma, anoAtual));
            if (parametroExecucao != null)
                return parametroExecucao.Ativo;

            return false;
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            if (ano < DateTime.Now.Year)
                parametroExecucao.Ano = ano;

            parametroExecucao.Valor = DateTime.Now.ToString();

            await mediator.Send(new AtualizarParametroSistemaCommand(parametroExecucao));
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
                {
                    await ConsolidarFrequenciasTurmasNoAno(ano);
                    await AtualizarDataExecucao(ano);
                }
            }
        }

        private async Task ConsolidarFrequenciasTurmasNoAno(int ano)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.ConsolidarFrequenciasTurmasNoAno, new FiltroAnoDto(ano), Guid.NewGuid(), null, fila: RotasRabbit.ConsolidarFrequenciasTurmasNoAno));
        }
    }
}
