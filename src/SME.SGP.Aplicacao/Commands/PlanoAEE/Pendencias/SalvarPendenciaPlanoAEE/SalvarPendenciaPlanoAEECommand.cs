using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaPlanoAEECommand : IRequest<bool>
    {
        public SalvarPendenciaPlanoAEECommand(long pendenciaId, long planoAEEId)
        {
            PendenciaId = pendenciaId;
            PlanoAEEId = planoAEEId;
        }

        public long PendenciaId { get; set; }
        public long PlanoAEEId { get; set; }
    }

}
