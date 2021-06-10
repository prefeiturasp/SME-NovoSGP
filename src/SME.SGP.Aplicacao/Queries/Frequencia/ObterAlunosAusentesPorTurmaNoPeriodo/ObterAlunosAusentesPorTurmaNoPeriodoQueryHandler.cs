using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAusentesPorTurmaNoPeriodoQueryHandler : IRequestHandler<ObterAlunosAusentesPorTurmaNoPeriodoQuery, IEnumerable<AlunoComponenteCurricularDto>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterAlunosAusentesPorTurmaNoPeriodoQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<AlunoComponenteCurricularDto>> Handle(ObterAlunosAusentesPorTurmaNoPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterAlunosAusentesPorTurmaEPeriodo(request.TurmaCodigo, request.DataInicio, request.DataFim);
    }
}
