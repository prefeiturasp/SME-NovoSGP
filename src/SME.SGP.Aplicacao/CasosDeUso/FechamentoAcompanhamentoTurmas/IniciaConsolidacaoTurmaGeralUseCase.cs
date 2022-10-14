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

        public async Task Executar(string turmaCodigo, int? bimestre, int? anoLetivo)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaSync, new FiltroConsolidacaoTurmaDto(turmaCodigo, bimestre, anoLetivo: anoLetivo), Guid.NewGuid(), null));
        }
    }
}
