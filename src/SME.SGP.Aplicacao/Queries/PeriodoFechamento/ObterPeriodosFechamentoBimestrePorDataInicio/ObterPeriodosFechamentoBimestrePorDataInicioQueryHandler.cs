
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
    public class ObterPeriodosFechamentoBimestrePorDataInicioQueryHandler : IRequestHandler<ObterPeriodosFechamentoBimestrePorDataInicioQuery, IEnumerable<PeriodoFechamentoBimestre>>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ObterPeriodosFechamentoBimestrePorDataInicioQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>> Handle(ObterPeriodosFechamentoBimestrePorDataInicioQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoFechamento.ObterPeriodosFechamentoBimestrePorDataInicio((int)request.Modalidade, request.DataAbertura);
    }
}
