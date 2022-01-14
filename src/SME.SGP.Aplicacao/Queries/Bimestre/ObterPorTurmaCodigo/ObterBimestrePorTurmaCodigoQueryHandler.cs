using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestrePorTurmaCodigoQueryHandler : IRequestHandler<ObterBimestrePorTurmaCodigoQuery, int>
    {
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IMediator mediator;

        public ObterBimestrePorTurmaCodigoQueryHandler(IConsultasPeriodoEscolar consultasPeriodoEscolar, IMediator mediator)
        {
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<int> Handle(ObterBimestrePorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));
            return await consultasPeriodoEscolar.ObterBimestre(request.Data, turma.ModalidadeCodigo, turma.Semestre);
        }
    }
}
