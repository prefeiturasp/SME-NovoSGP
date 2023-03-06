using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPorAlunoDisciplinaDataQueryHandler : IRequestHandler<ObterPorAlunoDisciplinaDataQuery, FrequenciaAluno>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterPorAlunoDisciplinaDataQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<FrequenciaAluno> Handle(ObterPorAlunoDisciplinaDataQuery request, CancellationToken cancellationToken)
        {
            return repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDisciplinaData(request.CodigoAluno, request.DisciplinaId, request.DataAtual, request.TurmaId);
        }
    }
}
