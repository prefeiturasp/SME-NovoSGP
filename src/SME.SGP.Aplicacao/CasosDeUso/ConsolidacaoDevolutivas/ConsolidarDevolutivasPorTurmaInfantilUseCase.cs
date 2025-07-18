using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<string>();
            var ueId = long.Parse(filtro);

            await ConsolidarDevolutivasAnoAtual(ueId);

            await ConsolidarDevolutivasHistorico(ueId);

            return true;

        }

        private async Task ConsolidarDevolutivasAnoAtual(long ueId)
        {
            var anoAtual = DateTime.Now.Year;

            var turmasInfantil = await mediator
                .Send(new ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery(anoAtual, ueId));

            await PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(turmasInfantil, anoAtual);

            await AtualizarDataExecucao(anoAtual);
        }

        private async Task ConsolidarDevolutivas(int ano, long ueId)
        {
            var turmasInfantil = await mediator.Send(new ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery(ano, ueId));

            await PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(turmasInfantil, ano);

            await AtualizarDataExecucao(ano);
        }

        private async Task ConsolidarDevolutivasHistorico(long ueId)
        {
            for (var ano = 2021; ano < DateTime.Now.Year; ano++)
            {
                if (!await mediator.Send(new ExisteConsolidacaoDevolutivaTurmaPorAnoQuery(ano)))
                {
                    await ConsolidarDevolutivas(ano, ueId);
                }
            }
        }

        private async Task PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(IEnumerable<DevolutivaTurmaDTO> turmasInfantil, int anoLetivo)
        {
            if (turmasInfantil == null || !turmasInfantil.Any())
                throw new NegocioException("Não foi possível localizar turmas para consolidar dados de devolutivas");

            foreach (var turma in turmasInfantil)
            {
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDevolutivasPorTurma, new FiltroDevolutivaTurmaDTO(turma.TurmaId, anoLetivo), Guid.NewGuid(), null));
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Publicar Mensagem Consolidar Devolutivas Por Turmas Infantil", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
                }

            }
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, ano));
            if (parametroSistema.NaoEhNulo())
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");
                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}