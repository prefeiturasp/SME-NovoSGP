using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQueryHandler : IRequestHandler<ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery, FrequenciaAluno>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;
        
        public ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo;
        }

        public async Task<FrequenciaAluno> Handle(ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterAsync(request.CodigoAluno, request.DisciplinaId, request.PeriodoEscolarId, request.Tipo, request.TurmaId);
    }
}