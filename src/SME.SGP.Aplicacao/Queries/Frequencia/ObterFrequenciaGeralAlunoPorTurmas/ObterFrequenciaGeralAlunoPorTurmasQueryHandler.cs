using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorTurmasQueryHandler : IRequestHandler<ObterFrequenciaGeralAlunoPorTurmasQuery, FrequenciaAluno>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaGeralAlunoPorTurmasQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<FrequenciaAluno> Handle(ObterFrequenciaGeralAlunoPorTurmasQuery request, CancellationToken cancellationToken)
        {
            var frequenciaAlunoPeriodos = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaGeralAlunoPorTurmas(request.CodigoAluno, request.CodigosTurmas, request.TipoCalendarioId);

            if (frequenciaAlunoPeriodos == null || !frequenciaAlunoPeriodos.Any())
                return null;

            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = frequenciaAlunoPeriodos.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciaAlunoPeriodos.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciaAlunoPeriodos.Sum(f => f.TotalCompensacoes),
            };

            return frequenciaAluno;
        }
    }
}
