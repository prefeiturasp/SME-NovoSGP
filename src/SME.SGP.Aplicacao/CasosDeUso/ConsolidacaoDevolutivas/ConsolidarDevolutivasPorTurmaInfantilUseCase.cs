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
            var anoAtual = DateTime.Now.Year;
            var turmasInfantil = await mediator.Send(new ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery(anoAtual));
            var turmasIds = turmasInfantil.Select(x => x.Id).ToArray();

            await mediator.Send(new LimparConsolidacaoDevolutivasCommand(turmasIds));

            var agrupamentoTurmaUe = turmasInfantil.GroupBy(x => x.UeId);
            
            await PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(anoAtual, agrupamentoTurmaUe);

            await AtualizarDataExecucao(anoAtual);
        }

        private async Task ConsolidarDevolutivas(int ano)
        {
            var turmasInfantil = await mediator.Send(new ObterTurmasComDevolutivaPorModalidadeInfantilEAnoQuery(ano));
            var turmasIds = turmasInfantil.Select(x => x.Id).ToArray();
            await mediator.Send(new LimparConsolidacaoDevolutivasCommand(turmasIds));

            var agrupamentoTurmaUe = turmasInfantil.GroupBy(x => x.UeId);
            await PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(ano, agrupamentoTurmaUe);

            await AtualizarDataExecucao(ano);
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

        private async Task PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(int anoLetivo, IEnumerable<IGrouping<long, DevolutivaTurmaDTO>> agrupamentoTurmaUe)
        {
            if (agrupamentoTurmaUe == null && !agrupamentoTurmaUe.Any())
                throw new NegocioException("Não foi possível localizar turmas para consolidar dados de devolutivas");

            foreach (var turmaUe in agrupamentoTurmaUe)
            {
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDevolutivasPorTurma, new FiltroDevolutivaTurmaDTO(turmaUe.FirstOrDefault().TurmaId, anoLetivo, turmaUe.FirstOrDefault().UeId), Guid.NewGuid(), null));
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Publicar Mensagem Consolidar Devolutivas Por Turmas Infantil", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));                    
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

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma, ano));
            if (parametroSistema != null)
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");
                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}