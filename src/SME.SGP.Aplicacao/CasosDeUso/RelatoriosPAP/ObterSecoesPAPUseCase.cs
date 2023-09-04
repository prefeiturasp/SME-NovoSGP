using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSecoesPAPUseCase : IObterSecoesPAPUseCase
    {
        private readonly IMediator mediator;

        public ObterSecoesPAPUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<SecaoTurmaAlunoPAPDto> Executar(FiltroObterSecoesDto param)
        {
            return this.mediator.Send(new ObterSecoesPAPQuery(param.CodigoTurma, param.CodigoAluno, param.PAPPeriodoId));
        }
    }
}
