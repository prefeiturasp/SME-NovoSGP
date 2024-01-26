using MediatR;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAlunosAusentesUseCase : IObterTurmasAlunosAusentesUseCase
    {
        private readonly IMediator mediator;

        public ObterTurmasAlunosAusentesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<IEnumerable<AlunosAusentesDto>> Executar(FiltroObterAlunosAusentesDto param)
        {
            return this.mediator.Send(new ObterTurmasAlunosAusentesQuery(param));
        }
    }
}
