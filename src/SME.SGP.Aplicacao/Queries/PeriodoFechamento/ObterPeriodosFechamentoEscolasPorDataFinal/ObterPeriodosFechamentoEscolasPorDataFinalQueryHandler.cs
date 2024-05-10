using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosFechamentoEscolasPorDataFinalQueryHandler : IRequestHandler<ObterPeriodosFechamentoEscolasPorDataFinalQuery, IEnumerable<PeriodoFechamentoBimestre>>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ObterPeriodosFechamentoEscolasPorDataFinalQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>> Handle(ObterPeriodosFechamentoEscolasPorDataFinalQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoFechamento.ObterPeriodosFechamentoEscolasPorDataFinal(request.DataFinal);
    }
}
