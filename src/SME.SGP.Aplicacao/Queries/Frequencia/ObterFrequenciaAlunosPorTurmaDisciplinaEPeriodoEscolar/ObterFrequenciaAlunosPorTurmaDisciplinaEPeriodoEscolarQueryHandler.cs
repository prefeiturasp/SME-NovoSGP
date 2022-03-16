using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQueryHandler : IRequestHandler<ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo;
        }

        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolar(request.Turma.CodigoTurma, request.ComponenteCurricularId.ToString(), TipoFrequenciaAluno.PorDisciplina, request.PeriodosEscolaresIds);
    }
}