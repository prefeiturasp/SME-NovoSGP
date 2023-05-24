using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ObterPeridosEscolaresPorTipoCalendarioIdQueryHandler : IRequestHandler<ObterPeridosEscolaresPorTipoCalendarioIdQuery, IEnumerable<PeriodoEscolar>>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterPeridosEscolaresPorTipoCalendarioIdQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<IEnumerable<PeriodoEscolar>> Handle(ObterPeridosEscolaresPorTipoCalendarioIdQuery request,
            CancellationToken cancellationToken)
        {
            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(request.TipoCalendarioId);
            
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException(MensagemNegocioPeriodo.PERIODO_ESCOLAR_NAO_ENCONTRADO);
            
            return periodosEscolares;
        }
    }
}
