using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoPorAulaIdQuery : IRequest<DiarioBordo>
    {

        public ObterDiarioBordoPorAulaIdQuery(long aulaId,long componenteCurricularId)
        {
            AulaId = aulaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long AulaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}
