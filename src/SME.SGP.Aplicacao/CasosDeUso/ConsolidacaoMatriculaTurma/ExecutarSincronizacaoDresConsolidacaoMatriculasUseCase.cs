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
            var ues = await mediator.Send(new ObterUesCodigosPorDreQuery(dre.Id));
            foreach (var ueCodigo in ues)
            {
                var ueDto = new FiltroConsolidacaoMatriculaUeDto(ueCodigo, dre.AnosAnterioresParaConsolidar);
                try
                {
                    var publicarTratamentoCiclo = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.ConsolidacaoMatriculasTurmasCarregar, ueDto, new Guid(), null, fila: RotasRabbit.ConsolidacaoMatriculasTurmasCarregar));
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
