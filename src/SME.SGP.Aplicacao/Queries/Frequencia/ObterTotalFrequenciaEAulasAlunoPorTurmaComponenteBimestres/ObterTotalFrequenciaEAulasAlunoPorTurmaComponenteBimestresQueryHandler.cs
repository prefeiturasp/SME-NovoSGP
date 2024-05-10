using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestresQueryHandler : IRequestHandler<ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestresQuery, IEnumerable<TotalFrequenciaEAulasAlunoDto>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodoConsulta;

        public ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestresQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodoConsulta)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodoConsulta = repositorioFrequenciaAlunoDisciplinaPeriodoConsulta ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodoConsulta));
        }

        public async Task<IEnumerable<TotalFrequenciaEAulasAlunoDto>> Handle(ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestresQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodoConsulta.ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestres(request.AlunoCodigo,
                                                                                                                                    request.TipoCalendarioId,
                                                                                                                                    request.ComponentesCurricularesIds,
                                                                                                                                    request.TurmasCodigo,
                                                                                                                                    request.Bimestre);       
    }
}
