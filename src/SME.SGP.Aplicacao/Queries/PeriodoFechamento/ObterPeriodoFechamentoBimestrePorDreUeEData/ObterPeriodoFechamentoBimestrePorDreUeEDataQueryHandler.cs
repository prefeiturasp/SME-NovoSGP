using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoBimestrePorDreUeEDataQueryHandler : IRequestHandler<ObterPeriodoFechamentoBimestrePorDreUeEDataQuery, PeriodoFechamentoBimestre>
    {
        private readonly IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre;

        public ObterPeriodoFechamentoBimestrePorDreUeEDataQueryHandler(IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre)
        {
            this.repositorioPeriodoFechamentoBimestre = repositorioPeriodoFechamentoBimestre ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamentoBimestre));
        }

        public async Task<PeriodoFechamentoBimestre> Handle(ObterPeriodoFechamentoBimestrePorDreUeEDataQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoFechamentoBimestre.ObterPeridoFechamentoBimestrePorDreUeEData(request.ModalidadeTipoCalendario,
                                                                                                     request.DataInicio,
                                                                                                     request.Bimestre,
                                                                                                     request.DreId,
                                                                                                     request.UeId);
    }
}
