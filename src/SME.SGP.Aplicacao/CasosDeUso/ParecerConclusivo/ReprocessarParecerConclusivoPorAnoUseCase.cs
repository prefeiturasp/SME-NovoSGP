using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarParecerConclusivoPorAnoUseCase : AbstractUseCase, IReprocessarParecerConclusivoPorAnoUseCase
    {
        public ReprocessarParecerConclusivoPorAnoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(int anoLetivo)
        {
            var dres = await mediator.Send(ObterIdsDresQuery.Instance);
            foreach(var dreId in dres)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaAtualizarParecerConclusivoAlunoPorDre, new FiltroDreUeTurmaDto(anoLetivo, dreId), Guid.NewGuid(), null));
            }
        }
    }
}
