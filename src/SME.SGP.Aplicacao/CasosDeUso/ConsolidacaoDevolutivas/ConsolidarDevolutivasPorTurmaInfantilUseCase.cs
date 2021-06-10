using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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
            var turmasInfantil = await mediator.Send(new ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery(anoAtual));
            
            await mediator.Send(new LimparConsolidacaoDevolutivasCommand(anoAtual));

            await PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(turmasInfantil, anoAtual);

            await AtualizarDataExecucao(anoAtual);

            return true;
        }

        private async Task PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(IEnumerable<DevolutivaTurmaDTO> turmasInfantil, int anoLetivo)
        {
            if (turmasInfantil == null && !turmasInfantil.Any())
                throw new NegocioException("Não foi possível localizar turmas para consolidar dados de devolutivas");
           
            foreach (var turma in turmasInfantil)
            {
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDevolutivasPorTurma, new FiltroDevolutivaTurmaDTO(turma.TurmaId, anoLetivo), Guid.NewGuid(), null));
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