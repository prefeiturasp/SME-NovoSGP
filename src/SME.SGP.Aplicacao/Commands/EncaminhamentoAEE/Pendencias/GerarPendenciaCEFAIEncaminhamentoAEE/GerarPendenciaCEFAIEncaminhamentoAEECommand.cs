using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCEFAIEncaminhamentoAEECommand : IRequest<bool>
    {
        public EncaminhamentoAEE EncaminhamentoAEE { get; set; }
        
        public GerarPendenciaCEFAIEncaminhamentoAEECommand(EncaminhamentoAEE encaminhamentoAEE)
        {
            EncaminhamentoAEE = encaminhamentoAEE;
        }
    }
}
