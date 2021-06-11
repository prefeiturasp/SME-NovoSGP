using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand : IRequest<bool>
    {
        public EncaminhamentoAEE EncaminhamentoAEE { get; set; }
        public bool EhCEFAI { get; set; }


        public GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(EncaminhamentoAEE encaminhamentoAEE, bool ehCEFAI)
        {
            EncaminhamentoAEE = encaminhamentoAEE;
            EhCEFAI = ehCEFAI;
        }
    }
}
