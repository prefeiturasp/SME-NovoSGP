using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulasPorTipoQueryHandler : IRequestHandler<ObterPendenciasAulasPorTipoQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciasAulasPorTipoQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<IEnumerable<Aula>> Handle(ObterPendenciasAulasPorTipoQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.ListarPendenciasPorTipo(request.TipoPendenciaAula, request.TabelaReferencia, request.Modalidades, request.DreId, request.AnoLetivo);
    }
}
