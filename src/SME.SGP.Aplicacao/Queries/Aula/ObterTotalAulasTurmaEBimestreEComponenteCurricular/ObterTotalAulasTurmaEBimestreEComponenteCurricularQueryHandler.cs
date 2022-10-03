using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasTurmaEBimestreEComponenteCurricularQueryHandler : IRequestHandler<ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery, IEnumerable<TurmaComponenteQntAulasDto>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterTotalAulasTurmaEBimestreEComponenteCurricularQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo;
        }

        public async Task<IEnumerable<TurmaComponenteQntAulasDto>> Handle(ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterTotalAulasPorDisciplinaETurmaEBimestre(request.TurmasCodigo, request.ComponentesCurricularesId, request.TipoCalendarioId, request.Bimestres);
    }
}