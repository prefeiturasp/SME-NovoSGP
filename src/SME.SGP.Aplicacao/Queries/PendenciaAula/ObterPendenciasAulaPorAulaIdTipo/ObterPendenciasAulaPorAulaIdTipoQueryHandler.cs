using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdTipoQueryHandler : IRequestHandler<ObterPendenciasAulaPorAulaIdTipoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAulaConsulta;

        public ObterPendenciasAulaPorAulaIdTipoQueryHandler(IRepositorioPendenciaAulaConsulta repositorio)
        {
            this.repositorioPendenciaAulaConsulta = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<long>> Handle(ObterPendenciasAulaPorAulaIdTipoQuery request, CancellationToken cancellationToken)
                  => await repositorioPendenciaAulaConsulta.ObterPendenciaIdPorAula(request.AulaId, request.TipoPendencia);
    }
}

