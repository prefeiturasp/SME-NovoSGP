using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdQuery : IRequest<long[]>
    {
        public ObterPendenciasAulaPorAulaIdQuery(long aulaId, Modalidade modalidade)
        {
            AulaId = aulaId;
            EhModalidadeInfantil = modalidade == Modalidade.Infantil;
        }

        public long AulaId { get; set; }

        public bool EhModalidadeInfantil { get; internal set; }
    }
}
