using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPAPUseCase : AbstractUseCase, ISalvarRelatorioPAPUseCase
    {
        public SalvarRelatorioPAPUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ResultadoRelatorioPAPDto> Executar(RelatorioPAPDto relatorioPAPDto)
            => await mediator.Send(new PersistirRelatorioPAPCommand(relatorioPAPDto));
    }
}