using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryHandler : IRequestHandler<ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery, FrequenciaAluno>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }
        public async Task<FrequenciaAluno> Handle(ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery request, CancellationToken cancellationToken)
         => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoBimestreAsync(request.CodigoAluno, request.Bimestre, request.TipoFrequencia, request.CodigoTurma, request.DisciplinasId, request.Professor);
    }
}
