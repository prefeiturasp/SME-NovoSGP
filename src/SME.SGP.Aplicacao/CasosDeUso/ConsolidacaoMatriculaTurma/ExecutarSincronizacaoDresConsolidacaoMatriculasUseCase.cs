using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoDresConsolidacaoMatriculasUseCase : AbstractUseCase, IExecutarSincronizacaoDresConsolidacaoMatriculasUseCase
    {
        public ExecutarSincronizacaoDresConsolidacaoMatriculasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var dre = mensagem.ObterObjetoMensagem<FiltroConsolidacaoMatriculaDreDto>();
            var ues = await mediator.Send(new ObterUesIdsPorDreQuery(dre.Id));
            foreach (var ueId in ues)
            {
                var ueDto = new FiltroConsolidacaoMatriculaUeDto(ueId, dre.AnosAnterioresParaConsolidar);
                try
                {
                    var publicarTratamentoCiclo = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidacaoMatriculasTurmasUeCarregar, ueDto, Guid.NewGuid(), null));
                    if (!publicarTratamentoCiclo)
                    {
                        var mensagemSentry = $"Não foi possível inserir a dre : {publicarTratamentoCiclo} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagemSentry);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
            return true;
        }
    }
}
