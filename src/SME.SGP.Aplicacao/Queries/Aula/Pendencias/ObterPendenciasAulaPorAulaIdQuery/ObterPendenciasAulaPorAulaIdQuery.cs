using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdQuery : IRequest<long[]>
    {
        public ObterPendenciasAulaPorAulaIdQuery(long aulaId, bool temAtividadeAvaliativa = false, bool ehModalidadeInfantil = false)
        {
            AulaId = aulaId;
            TemAtividadeAvaliativa = temAtividadeAvaliativa;
            EhModalidadeInfantil = ehModalidadeInfantil;
        }

        public long AulaId { get; set; }
        public bool EhModalidadeInfantil { get; set; }
        public bool TemAtividadeAvaliativa { get; set; }
    }
}
