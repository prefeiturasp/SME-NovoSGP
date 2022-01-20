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
    public class ObterIndicativoPendenciasAulasPorTipoQueryHandler : IRequestHandler<ObterIndicativoPendenciasAulasPorTipoQuery, bool>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterIndicativoPendenciasAulasPorTipoQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<bool> Handle(ObterIndicativoPendenciasAulasPorTipoQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, request.TipoPendenciaAula, request.TabelaReferencia, request.Modalidades, request.AnoLetivo);
    }
}
