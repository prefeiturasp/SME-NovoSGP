using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorPeriodosEscolaresQueryHandler : IRequestHandler<ObterFrequenciaGeralAlunoPorPeriodosEscolaresQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaGeralAlunoPorPeriodosEscolaresQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciaGeralAlunoPorPeriodosEscolaresQuery request, CancellationToken cancellationToken)
        {
            return repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaGeralAlunoPorPeriodosEscolares(request.CodigoAluno, request.CodigoTurma, request.IdsPeriodosEscolares);
        }
    }
}
