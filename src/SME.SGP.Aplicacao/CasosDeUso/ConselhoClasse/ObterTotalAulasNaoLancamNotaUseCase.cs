using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
