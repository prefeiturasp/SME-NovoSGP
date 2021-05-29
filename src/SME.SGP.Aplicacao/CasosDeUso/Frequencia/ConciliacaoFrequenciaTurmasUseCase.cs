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
            await ProcessarNaData(DateTime.Now, "");
        }

        public async Task ProcessarNaData(DateTime dataPeriodo, string turmaCodigo)
        {
            await mediator.Send(new ConciliacaoFrequenciaTurmasCommand(dataPeriodo, turmaCodigo, string.Empty));
        }
    }
}
