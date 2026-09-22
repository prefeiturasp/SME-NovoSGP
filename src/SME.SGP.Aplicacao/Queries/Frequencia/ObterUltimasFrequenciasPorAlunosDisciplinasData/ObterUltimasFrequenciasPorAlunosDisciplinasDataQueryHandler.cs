using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimasFrequenciasPorAlunosDisciplinasDataQueryHandler : IRequestHandler<ObterUltimasFrequenciasPorAlunosDisciplinasDataQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorio;

        public ObterUltimasFrequenciasPorAlunosDisciplinasDataQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<FrequenciaAluno>> Handle(ObterUltimasFrequenciasPorAlunosDisciplinasDataQuery request, CancellationToken cancellationToken)
            => repositorio.ObterUltimasFrequenciasPorAlunosDisciplinasDataAsync(request.CodigosAlunos,
                request.DisciplinasId, request.DataAtual, request.TurmaCodigo, request.Professor);
    }
}
