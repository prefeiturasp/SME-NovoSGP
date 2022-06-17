using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigoAulaInfantilPorTurmaCodigoUseCase : AbstractUseCase, IObterTurmaCodigoAulaInfantilPorTurmaCodigoUseCase
    {
        public ObterTurmaCodigoAulaInfantilPorTurmaCodigoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<DevolutivaTurmaDTO>();

                var turmasIds = await mediator.Send(new ObterTurmasComDevolutivaPorAulaTurmaIdQuery(filtro.TurmaId));

                if (turmasIds.Any() && turmasIds.FirstOrDefault() != 0)
                    await mediator.Send(new LimparConsolidacaoDevolutivasCommand(turmasIds.ToArray()));

                await PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(filtro);
                await AtualizarDataExecucao(filtro.AnoLetivo);
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Erro ao executar", LogNivel.Critico, LogContexto.Geral, ex.Message));
                return false;
            }
        }

        private async Task PublicarMensagemConsolidarDevolutivasPorTurmasInfantil(DevolutivaTurmaDTO turmaUe)
        {

            try
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarDevolutivasPorTurma, new FiltroDevolutivaTurmaDTO(turmaUe.TurmaId, turmaUe.AnoAtual, turmaUe.UeId), Guid.NewGuid(), null));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Publicar Mensagem Consolidar Devolutivas Por Turmas Infantil", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
            }

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
