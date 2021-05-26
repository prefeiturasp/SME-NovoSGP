using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdQueryHandler : IRequestHandler<ObterPendenciasAulaPorAulaIdQuery, long[]>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterPendenciasAulaPorAulaIdQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }
        public async Task<long[]> Handle(ObterPendenciasAulaPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            var pendencias = await repositorioPendenciaAula.PossuiPendenciasPorAulaId(request.AulaId, request.EhModalidadeInfantil);
            if (pendencias == null || !request.TemAtividadeAvaliativa)
                return null;

            pendencias = new PendenciaAulaDto
            {
                AulaId = request.AulaId,
                PossuiPendenciaFrequencia = pendencias.PossuiPendenciaFrequencia,
                PossuiPendenciaPlanoAula = pendencias.PossuiPendenciaPlanoAula,
                PossuiPendenciaAtividadeAvaliativa = false
            };
            pendencias.PossuiPendenciaAtividadeAvaliativa = request.TemAtividadeAvaliativa ? 
                await repositorioPendenciaAula.PossuiPendenciasAtividadeAvaliativaPorAulaId(request.AulaId) : false;

            return pendencias.RetornarTipoPendecias();
        }
    }
}
