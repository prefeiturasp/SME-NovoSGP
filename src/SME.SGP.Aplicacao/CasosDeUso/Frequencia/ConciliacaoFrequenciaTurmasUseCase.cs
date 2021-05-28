using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
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
            await ProcessarNaData(DateTime.Now);
        }

        public async Task ProcessarNaData(DateTime dataPeriodo)
        {
            await mediator.Send(new ConciliacaoFrequenciaTurmasCommand(dataPeriodo, string.Empty, string.Empty));
        }
    }
}
