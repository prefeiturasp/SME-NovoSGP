using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasNaoLancamNotaUseCase : IObterTotalAulasNaoLancamNotaUseCase
    {
        private readonly IMediator mediator;
        public ObterTotalAulasNaoLancamNotaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<TotalAulasNaoLancamNotaDto>> Executar(string codigoTurma, int bimestre, string codigoAluno)
        {
            return await mediator.Send(new ObterTotalAulasNaoLancamNotaQuery(codigoTurma,bimestre, codigoAluno));
        }
    }
}
