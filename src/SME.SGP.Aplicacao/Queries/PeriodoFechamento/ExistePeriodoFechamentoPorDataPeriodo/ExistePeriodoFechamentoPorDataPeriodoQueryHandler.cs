using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePeriodoFechamentoPorDataPeriodoQueryHandler : IRequestHandler<ExistePeriodoFechamentoPorDataPeriodoQuery, bool>
    {
        private readonly IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre;

        public ExistePeriodoFechamentoPorDataPeriodoQueryHandler(IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre)
        {
            this.repositorioPeriodoFechamentoBimestre = repositorioPeriodoFechamentoBimestre ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamentoBimestre));
        }

        public async Task<bool> Handle(ExistePeriodoFechamentoPorDataPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoFechamentoBimestre.ExistePeriodoFechamentoPorDataPeriodoIdEscolar(request.PeriodoEscolarId, request.DataReferencia);

    }
}
