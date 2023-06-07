using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryHandler : IRequestHandler<ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaAlunoDisciplinaPeriodo
                .ObterPorAlunoTurmasDisciplinasDataAsync(request.CodigoAluno, request.TipoFrequencia, request.DisciplinasId, request.TurmasCodigo, request.Bimestres, request.periodosEscolaresId);
        }
    }
}
