using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorAlunoDisciplinaData
{
    public class ObterFrequenciaPorAlunoDisciplinaDataQueryHandler : IRequestHandler<ObterFrequenciaPorAlunoDisciplinaDataQuery, FrequenciaAluno>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaPorAlunoDisciplinaDataQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<FrequenciaAluno> Handle(ObterFrequenciaPorAlunoDisciplinaDataQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDisciplinaDataAsync(request.CodigoAluno, request.DisciplinaId, request.DataAtual, request.TurmaCodigo);
    }
}
