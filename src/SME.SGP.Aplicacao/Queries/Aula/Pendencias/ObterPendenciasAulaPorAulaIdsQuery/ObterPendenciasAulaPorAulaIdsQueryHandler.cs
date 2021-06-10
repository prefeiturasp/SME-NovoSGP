using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdsQueryHandler : IRequestHandler<ObterPendenciasAulaPorAulaIdsQuery, bool>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterPendenciasAulaPorAulaIdsQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }
        public async Task<bool> Handle(ObterPendenciasAulaPorAulaIdsQuery request, CancellationToken cancellationToken)
        {
            var possuiPendencia = await repositorioPendenciaAula.PossuiPendenciasPorAulasId(request.AulasId, request.EhModalidadeInfantil);
            if (!possuiPendencia) {
                possuiPendencia = await repositorioPendenciaAula.PossuiAtividadeAvaliativaSemNotaPorAulasId(request.AulasId);
            }
            return possuiPendencia;
        }
    }
}
