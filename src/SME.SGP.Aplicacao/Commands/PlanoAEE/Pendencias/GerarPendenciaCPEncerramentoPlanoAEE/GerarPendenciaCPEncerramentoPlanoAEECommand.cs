using MediatR;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCPEncerramentoPlanoAEECommand : IRequest<bool>
    {
        public long PlanoAEEId { get; set; }


        public GerarPendenciaCPEncerramentoPlanoAEECommand(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }
    }

}
