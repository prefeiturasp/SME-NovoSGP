using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaEncaminhamentoAEECommand : IRequest<bool>
    {
        public ExcluirPendenciaEncaminhamentoAEECommand(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

}
