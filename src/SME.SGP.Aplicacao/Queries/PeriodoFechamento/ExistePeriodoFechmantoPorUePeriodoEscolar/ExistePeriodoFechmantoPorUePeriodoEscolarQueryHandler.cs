using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePeriodoFechmantoPorUePeriodoEscolarQueryHandler : IRequestHandler<ExistePeriodoFechmantoPorUePeriodoEscolarQuery, bool>
    {
        private readonly IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre;

        public ExistePeriodoFechmantoPorUePeriodoEscolarQueryHandler(IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre)
        {
            this.repositorioPeriodoFechamentoBimestre = repositorioPeriodoFechamentoBimestre ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamentoBimestre));
        }

        public async Task<bool> Handle(ExistePeriodoFechmantoPorUePeriodoEscolarQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoFechamentoBimestre.ExistePeriodoFechamentoPorUePeriodoEscolar(request.UeId, request.PeriodoEscolarId);
    }
}
