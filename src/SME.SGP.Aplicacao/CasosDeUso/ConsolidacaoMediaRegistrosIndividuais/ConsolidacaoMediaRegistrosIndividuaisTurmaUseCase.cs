using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ConsolidacaoMediaRegistrosIndividuaisTurmaUseCase : AbstractUseCase, IConsolidacaoMediaRegistrosIndividuaisTurmaUseCase
    {
        public ConsolidacaoMediaRegistrosIndividuaisTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (!await ExecutarConsolidacao())
                return false;

            await ConsolidarMediaRegistroIndividual();
            return true;
        }

        private async Task ConsolidarMediaRegistroIndividual()
        {
            var anoAtual = DateTime.Now.Year;

            var turmasInfantil = await mediator.Send(new ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery(anoAtual));

            await mediator.Send(new LimparConsolidacaoMediaRegistroIndividualCommand(anoAtual));

            await PublicarMensagemConsolidarMediaRegistrosIndividuaisPorAnoETurma(turmasInfantil, anoAtual);

            await AtualizarDataExecucao(anoAtual);
        }

        private async Task PublicarMensagemConsolidarMediaRegistrosIndividuaisPorAnoETurma(IEnumerable<RegistroIndividualDTO> turmasInfantil, int anoLetivo)
        {
            if (turmasInfantil == null && !turmasInfantil.Any())
                throw new NegocioException("Não foi possível localizar turmas para consolidar dados de Média de Registros Individuais");

            foreach (var turma in turmasInfantil)
            {
                try
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarMediaRegistrosIndividuais, new FiltroMediaRegistroIndividualTurmaDTO(turma.TurmaId, anoLetivo), Guid.NewGuid(), null));
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Consolidacao Media Registros Individuais Turma UseCase", LogNivel.Critico, LogContexto.ConsolidacaoMatricula, ex.Message));                    
                }
                
            }
        }

        private async Task<bool> ExecutarConsolidacao()
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoMediaRegistrosIndividuaisTurma, DateTime.Now.Year));
            if (parametroExecucao != null)
                return parametroExecucao.Ativo;

            return false;
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoMediaRegistrosIndividuaisTurma, ano));
            if (parametroSistema != null)
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");
                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}