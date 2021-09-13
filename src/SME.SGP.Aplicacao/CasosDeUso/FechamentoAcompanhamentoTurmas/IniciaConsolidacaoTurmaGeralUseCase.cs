using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IniciaConsolidacaoTurmaGeralUseCase : AbstractUseCase, IIniciaConsolidacaoTurmaGeralUseCase
    {
        public IniciaConsolidacaoTurmaGeralUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(string turmaCodigo, int? bimestre)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaSync, new FiltroConsolidacaoTurmaDto(turmaCodigo, bimestre), Guid.NewGuid(), null));
        }
    }
}
