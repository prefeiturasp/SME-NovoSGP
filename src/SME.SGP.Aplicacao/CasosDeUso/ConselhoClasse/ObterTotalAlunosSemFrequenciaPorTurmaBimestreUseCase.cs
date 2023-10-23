using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase : IObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase
    {
        private readonly IMediator mediator;
        public ObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<int>> Executar(string disciplinaId, string codigoTurma, int bimestre, DateTime dataMatricula)
        {
            return await mediator.Send(new ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery(disciplinaId, codigoTurma, bimestre, dataMatricula));
        }
    }
}
