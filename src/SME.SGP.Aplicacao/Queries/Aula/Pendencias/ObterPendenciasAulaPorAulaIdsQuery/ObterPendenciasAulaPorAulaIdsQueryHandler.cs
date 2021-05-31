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
            var possuiPendencia = false;
            foreach (long aula in request.AulasId)
            {
                var pendencias = await repositorioPendenciaAula.PossuiPendenciasPorAulaId(aula, request.EhModalidadeInfantil);
                if (pendencias != null)
                    if (pendencias.PossuiPendenciaFrequencia || pendencias.PossuiPendenciaDiarioBordo)
                        return possuiPendencia = true;
            }

            if (!possuiPendencia)
                possuiPendencia = await repositorioPendenciaAula.PossuiAtividadeAvaliativaSemNotaPorAulasId(request.AulasId);
            
            return possuiPendencia;
        }
    }
}
