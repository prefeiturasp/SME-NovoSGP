using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasRegistradasPorTurmasComponentesPeriodoQueryHandler : IRequestHandler<ObterFrequenciasRegistradasPorTurmasComponentesPeriodoQuery, IEnumerable<FrequenciaRegistradaDto>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciasRegistradasPorTurmasComponentesPeriodoQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo;
        }

        public async Task<IEnumerable<FrequenciaRegistradaDto>> Handle(ObterFrequenciasRegistradasPorTurmasComponentesPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaRegistradaPorTurmaDisciplinaEPeriodo(request.CodigosTurma, request.ComponentesCurricularesId.Select(cc => cc).ToArray(), request.PeriodosEscolaresId);
    }
}
