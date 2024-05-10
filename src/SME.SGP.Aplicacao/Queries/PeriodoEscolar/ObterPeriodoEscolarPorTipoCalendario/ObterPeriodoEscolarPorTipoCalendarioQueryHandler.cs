using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorTipoCalendarioQueryHandler :IRequestHandler<ObterPeriodoEscolarPorTipoCalendarioQuery,IEnumerable<PeriodoEscolar>>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioConsulta;

        public ObterPeriodoEscolarPorTipoCalendarioQueryHandler(IRepositorioPeriodoEscolarConsulta consulta)
        {
            repositorioConsulta = consulta ?? throw new ArgumentNullException(nameof(consulta));
        }

        public async Task<IEnumerable<PeriodoEscolar>> Handle(ObterPeriodoEscolarPorTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsulta.ObterPorTipoCalendario(request.TipoCalendarioId);
        }
    }
}