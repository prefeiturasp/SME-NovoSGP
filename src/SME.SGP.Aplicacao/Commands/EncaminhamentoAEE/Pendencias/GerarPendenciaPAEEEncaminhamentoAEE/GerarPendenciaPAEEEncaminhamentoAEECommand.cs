using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaPAEEEncaminhamentoAEECommand : IRequest<bool>
    {
        public EncaminhamentoAEE EncaminhamentoAEE { get; set; }
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }
        public string MotivoDevolucao { get; set; }


        public GerarPendenciaPAEEEncaminhamentoAEECommand(EncaminhamentoAEE encaminhamentoAEE)
        {
            EncaminhamentoAEE = encaminhamentoAEE;
        }
    }

}
