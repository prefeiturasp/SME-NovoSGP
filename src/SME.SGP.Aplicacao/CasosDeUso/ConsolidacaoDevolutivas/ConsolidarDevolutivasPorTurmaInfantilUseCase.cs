using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var anoAtual = DateTime.Now.Year;
            var turmas = await mediator.Send(new ObterTurmasComModalidadePorAnoQuery(anoAtual));
            var turmasInfantil = turmas.Where(t => t.ModalidadeInfantil == true);

            await mediator.Send(new LimparConsolidacaoDevolutivasCommand(anoAtual));

            await PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(turmasInfantil);

            await AtualizarDataExecucao(anoAtual);

            return true;
        }

        private async Task PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(IEnumerable<TurmaModalidadeDto> turmas)
        {
            if (turmas == null && !turmas.Any())
                throw new NegocioException("Não foi possível localizar turmas para consolidar dados de devolutivas");

            foreach (var turma in turmas)
            {
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.ConsolidarDevolutivasPorTurmaInfantil, turma.TurmaCodigo, Guid.NewGuid(), null, fila: RotasRabbit.ConsolidarDevolutivasPorTurmaInfantil));
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, ano));
            if (parametroSistema != null)
            {
                parametroSistema.Valor = DateTime.Now.ToString();
                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}