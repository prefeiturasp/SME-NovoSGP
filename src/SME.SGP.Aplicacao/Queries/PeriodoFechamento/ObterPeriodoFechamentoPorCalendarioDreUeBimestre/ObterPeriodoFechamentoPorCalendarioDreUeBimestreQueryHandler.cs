using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoPorCalendarioDreUeBimestreQueryHandler : IRequestHandler<ObterPeriodoFechamentoPorCalendarioDreUeBimestreQuery, PeriodoFechamentoBimestre>
    {
        private readonly IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre;

        public ObterPeriodoFechamentoPorCalendarioDreUeBimestreQueryHandler(IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre)
        {
            this.repositorioPeriodoFechamentoBimestre = repositorioPeriodoFechamentoBimestre ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamentoBimestre));
        }

        public Task<PeriodoFechamentoBimestre> Handle(ObterPeriodoFechamentoPorCalendarioDreUeBimestreQuery request, CancellationToken cancellationToken)
            => repositorioPeriodoFechamentoBimestre.ObterPeriodoFechamanentoPorCalendarioDreUeBimestre(request.TipoCalendarioId,
                                                                                                       request.Bimestre,
                                                                                                       request.DreId,
                                                                                                       request.UeId);
    }
}
