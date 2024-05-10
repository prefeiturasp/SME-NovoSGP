using SME.SGP.Aplicacao.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase : AbstractUseCase, IReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase
    {
        public ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<FiltroDiarioBordoPendenciaDevolutivaDto>();
                var turmas = await mediator.Send(new ObterCodigosTurmasPorAnoModalidadeUeQuery(filtro.AnoLetivo, Modalidade.EducacaoInfantil, filtro.UeId));
                foreach (var turmaId in turmas)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorComponente,
                        new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo: filtro.AnoLetivo, dreId: filtro.DreId, ueCodigo: filtro.UeCodigo, turmaId: turmaId, ueId:filtro.UeId), Guid.NewGuid(), null));
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a verificação de pendencias de devolutivas por Turma", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
                return false;
            }
        }
    }
}
