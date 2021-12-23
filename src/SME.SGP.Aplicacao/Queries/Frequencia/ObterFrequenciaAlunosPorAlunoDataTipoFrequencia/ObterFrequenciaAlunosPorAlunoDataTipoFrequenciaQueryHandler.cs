using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQueryHandler : IRequestHandler<ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQuery, FrequenciaAluno>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo;
        }

        public async Task<FrequenciaAluno> Handle(ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDataAsync(request.CodigoAluno, request.DataReferencia, request.Tipo);
    }
}
