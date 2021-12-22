using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQueryHandler : IRequestHandler<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery request, CancellationToken cancellationToken)
        {
           return await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoTurmaComponenteBimestres(request.AlunoCodigo,
                                                                                 TipoFrequenciaAluno.PorDisciplina,
                                                                                 request.ComponenteCurricularId,
                                                                                 request.TurmaCodigo, request.Bimestres);
        }
    }
}
