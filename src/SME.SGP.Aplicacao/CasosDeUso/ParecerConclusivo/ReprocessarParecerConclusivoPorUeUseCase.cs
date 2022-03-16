using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarParecerConclusivoPorUeUseCase : AbstractUseCase, IReprocessarParecerConclusivoPorUeUseCase
    {
        public ReprocessarParecerConclusivoPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroDreUeTurmaDto>();
            var turmas = await mediator.Send(new ObterCodigosTurmasPorUeAnoQuery(filtro.AnoLetivo, filtro.UeCodigo));
            foreach (var turmaCodigo in turmas)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtualizarParecerConclusivoAlunoPorTurma
                    , new FiltroDreUeTurmaDto(filtro.AnoLetivo, filtro.DreId, filtro.UeCodigo, turmaCodigo), Guid.NewGuid(), null));

            return true;
        }
    }
}
