using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalFrequenciaEAulasPorPeriodoQueryHandler : IRequestHandler<ObterTotalFrequenciaEAulasPorPeriodoQuery, TotalFrequenciaEAulasPorPeriodoDto>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterTotalFrequenciaEAulasPorPeriodoQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<TotalFrequenciaEAulasPorPeriodoDto> Handle(ObterTotalFrequenciaEAulasPorPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterTotalFrequenciaEAulasPorPeriodo(request.AnoLetivo,
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
