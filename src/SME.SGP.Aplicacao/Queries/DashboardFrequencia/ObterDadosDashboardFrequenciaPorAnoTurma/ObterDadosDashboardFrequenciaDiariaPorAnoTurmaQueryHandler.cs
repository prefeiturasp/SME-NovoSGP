using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQueryHandler : IRequestHandler<ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery, IEnumerable<FrequenciaAlunoDashboardDto>>
    {
        private readonly IRepositorioDashBoardFrequencia repositorioFrequencia;

        public ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQueryHandler(IRepositorioDashBoardFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<FrequenciaAlunoDashboardDto>> Handle(ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterFrequenciasDiariaConsolidadas(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade, request.Semestre, request.TurmaIds, request.DataAula, request.VisaoDre);
    }
}
