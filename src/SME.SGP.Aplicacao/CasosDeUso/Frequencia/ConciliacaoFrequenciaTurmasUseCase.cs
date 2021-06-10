using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmasUseCase
    {
        public ConciliacaoFrequenciaTurmasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            await mediator.Send(new ConciliacaoFrequenciaTurmasCommand(DateTime.Now));
        }
    }
}
