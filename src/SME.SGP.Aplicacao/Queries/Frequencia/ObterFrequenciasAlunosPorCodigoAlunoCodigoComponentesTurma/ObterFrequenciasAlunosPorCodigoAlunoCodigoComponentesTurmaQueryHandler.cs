using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQueryHandler : IRequestHandler<ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaAsync(request.AlunoCodigo, request.TurmasCodigos, request.ComponenteCurricularCodigos);
    }
}
