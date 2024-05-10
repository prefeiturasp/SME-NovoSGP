using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.Aula.ObterAulasTurmaEBimestreEComponenteCurricular
{
    public class ObterAulasTurmaEBimestreEComponenteCurricularQueryHandler : IRequestHandler<ObterAulasTurmaEBimestreEComponenteCurricularQuery, IEnumerable<TurmaDataAulaComponenteQtdeAulasDto>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterAulasTurmaEBimestreEComponenteCurricularQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo;
        }

        public async Task<IEnumerable<TurmaDataAulaComponenteQtdeAulasDto>> Handle(ObterAulasTurmaEBimestreEComponenteCurricularQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterAulasPorDisciplinaETurmaEBimestre(request.TurmasCodigo, request.CodigosAlunos, request.ComponentesCurricularesId, request.TipoCalendarioId, request.Bimestres, request.DataMatriculaAluno, request.DataSituacaoAluno);
    }    
}
