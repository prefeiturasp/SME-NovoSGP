using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalFrequenciaEAulasPorPeriodoQueryHandler : IRequestHandler<ObterTotalFrequenciaEAulasPorPeriodoQuery, IEnumerable<TotalFrequenciaEAulasPorPeriodoDto>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterTotalFrequenciaEAulasPorPeriodoQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<TotalFrequenciaEAulasPorPeriodoDto>> Handle(ObterTotalFrequenciaEAulasPorPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterTotalFrequenciaEAulasPorPeriodo(request.AnoLetivo,
                                                                                    request.DreId,
                                                                                    request.UeId,
                                                                                    request.Modalidade,
                                                                                    request.Semestre,
                                                                                    request.DataInicio,
                                                                                    request.DataFim,
                                                                                    request.Mes,
                                                                                    request.TipoPeriodoDashboard);
    }
}
