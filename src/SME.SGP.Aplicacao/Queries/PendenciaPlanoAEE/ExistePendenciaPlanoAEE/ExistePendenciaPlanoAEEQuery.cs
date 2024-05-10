using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaPlanoAEEQuery : IRequest<bool>
    {
        public ExistePendenciaPlanoAEEQuery(long planoAeeId)
        {
            PlanoAeeId = planoAeeId;
        }

        public long PlanoAeeId { get; private set; }
    }
}
