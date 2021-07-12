using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorAnoTurmaQueryHandler : IRequestHandler<ObterDadosDashboardFrequenciaPorAnoTurmaQuery, IEnumerable<FrequenciaAlunoDashboardDto>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterDadosDashboardFrequenciaPorAnoTurmaQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<FrequenciaAlunoDashboardDto>> Handle(ObterDadosDashboardFrequenciaPorAnoTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterFrequenciasConsolidadasPorTurmaEAno(request.AnoLetivo,
                                                                                    request.DreId,
                                                                                    request.UeId,
                                                                                    request.Modalidade,
                                                                                    request.Semestre,
                                                                                    request.AnoTurma,
                                                                                    request.DataInicio,
                                                                                    request.DataFim,
                                                                                    request.Mes,
                                                                                    request.TipoPeriodoDashboard);
    }
}
