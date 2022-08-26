using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaPendenciaPlanoAEEQuery : IRequest<Pendencia>
    {
        public ObterUltimaPendenciaPlanoAEEQuery(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; set; }
    }
}
