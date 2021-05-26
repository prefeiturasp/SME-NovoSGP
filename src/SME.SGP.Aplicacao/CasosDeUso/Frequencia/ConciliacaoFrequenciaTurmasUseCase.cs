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

        public async Task Executar(DateTime? dataPeriodo = null)
        {
            var dataReferencia = dataPeriodo.HasValue ? dataPeriodo.Value : DateTime.Now;
            await mediator.Send(new ConciliacaoFrequenciaTurmasCommand(dataReferencia));
        }
    }
}
