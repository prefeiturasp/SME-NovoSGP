
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
    public class ObterPeriodosFechamentoBimestrePorDataFinalQueryHandler : IRequestHandler<ObterPeriodosFechamentoBimestrePorDataFinalQuery, IEnumerable<PeriodoFechamentoBimestre>>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ObterPeriodosFechamentoBimestrePorDataFinalQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>> Handle(ObterPeriodosFechamentoBimestrePorDataFinalQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoFechamento.ObterPeriodosFechamentoBimestrePorDataFinal((int)request.Modalidade, request.DataEncerramento);
    }
}
