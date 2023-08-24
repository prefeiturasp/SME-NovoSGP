using MediatR;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaTurmaPAPQueryHandler : IRequestHandler<ObterFrequenciaTurmaPAPQuery, FrequenciaAlunoDto>
    {
        private readonly IMediator mediator;

        public ObterFrequenciaTurmaPAPQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<FrequenciaAlunoDto> Handle(ObterFrequenciaTurmaPAPQuery request, CancellationToken cancellationToken)
        {
            var idsPeriodosEscolares = request.PeriodoRelatorio.PeriodosEscolaresRelatorio.Select(periodo => periodo.PeriodoEscolarId).ToArray();
            var frequenciasAluno = await mediator.Send(new ObterFrequenciaGeralAlunoPorPeriodosEscolaresQuery(request.CodigoAluno, request.CodigoTurma, idsPeriodosEscolares));

            return new FrequenciaAlunoDto()
            {
                TotalAulas = frequenciasAluno.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciasAluno.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciasAluno.Sum(f => f.TotalCompensacoes),
                TotalPresencas = frequenciasAluno.Sum(f => f.TotalPresencas),
                TotalRemotos = frequenciasAluno.Sum(f => f.TotalRemotos),
                AlunoCodigo = request.CodigoAluno
            };
        }
    }
}
