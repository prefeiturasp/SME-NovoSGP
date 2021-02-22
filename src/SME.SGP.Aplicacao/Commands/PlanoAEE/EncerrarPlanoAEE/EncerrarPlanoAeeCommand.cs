using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class EncerrarPlanoAeeCommand : IRequest<RetornoEncerramentoPlanoAEEDto>
    {
        public long PlanoId { get; set; }

        public EncerrarPlanoAeeCommand(long planoId)
        {
            PlanoId = planoId;
        }
    }
}
