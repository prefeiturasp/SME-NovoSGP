using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdQuery : IRequest<long[]>
    {
        public ObterPendenciasAulaPorAulaIdQuery(long aulaId, bool temAtividadeAvaliativa = false)
        {
            AulaId = aulaId;
            TemAtividadeAvaliativa = temAtividadeAvaliativa;
        }

        public long AulaId { get; set; }
        public bool EhModalidadeInfantil { get; internal set; }
        public bool TemAtividadeAvaliativa { get; set; }
    }
}
