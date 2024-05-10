using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresPorTipoCalendarioQueryHandler : IRequestHandler<ObterBimestresPorTipoCalendarioQuery, IEnumerable<int>>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterBimestresPorTipoCalendarioQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<IEnumerable<int>> Handle(ObterBimestresPorTipoCalendarioQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterBimestresPorTipoCalendario(request.TipoCalendarioId);
    }
}
