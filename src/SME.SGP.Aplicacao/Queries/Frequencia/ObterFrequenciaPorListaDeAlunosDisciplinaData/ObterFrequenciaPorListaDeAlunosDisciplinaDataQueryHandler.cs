using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPorListaDeAlunosDisciplinaDataQueryHandler : IRequestHandler<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery,IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaPorListaDeAlunosDisciplinaDataQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta frequenciaAlunoDisciplinaPeriodo)
        {
            repositorioFrequenciaAlunoDisciplinaPeriodo = frequenciaAlunoDisciplinaPeriodo ??
                                                          throw new ArgumentNullException(
                                                              nameof(frequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaAlunoDisciplinaPeriodo
                .ObterFrequenciaPorListaDeAlunosDisciplinaData(request.CodigosAlunos, request.DisciplinaId, request.PeriodoEscolarId, request.TurmaCodigo, request.Professor);
        }
    }
}