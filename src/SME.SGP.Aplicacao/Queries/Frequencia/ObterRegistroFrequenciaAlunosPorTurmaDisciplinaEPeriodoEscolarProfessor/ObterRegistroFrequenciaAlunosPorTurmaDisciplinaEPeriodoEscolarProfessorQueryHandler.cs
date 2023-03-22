using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQueryHandler : IRequestHandler<ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQuery, IEnumerable<FrequenciaAlunoDto>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo;
        }

        public async Task<IEnumerable<FrequenciaAlunoDto>> Handle(ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessorQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterRegistroFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarProfessor(request.Turma.CodigoTurma, request.ComponentesCurricularesId.Select(cc => cc.ToString()).ToArray(), request.PeriodosEscolaresIds, request.RfProfessorTerritorioSaber);
    }
}