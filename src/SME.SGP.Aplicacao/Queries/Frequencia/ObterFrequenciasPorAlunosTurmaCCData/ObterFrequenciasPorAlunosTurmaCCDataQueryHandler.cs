using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPorAlunosTurmaCCDataQueryHandler : IRequestHandler<ObterFrequenciasPorAlunosTurmaCCDataQuery, IEnumerable<FrequenciaAluno>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciasPorAlunosTurmaCCDataQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo )
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }
        public async Task<IEnumerable<FrequenciaAluno>> Handle(ObterFrequenciasPorAlunosTurmaCCDataQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunosDataAsync(request.AlunosCodigo, request.DataReferencia, request.TipoFrequencia, request.TurmaCodigo, request.ComponenteCurriularId);            
        }
    }
}
