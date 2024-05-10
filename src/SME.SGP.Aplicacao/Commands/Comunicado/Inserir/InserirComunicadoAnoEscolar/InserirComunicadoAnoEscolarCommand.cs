using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoAnoEscolarCommand : IRequest<bool>
    {
        public InserirComunicadoAnoEscolarCommand(Comunicado comunicado)
        {
            Comunicado = comunicado;
        }

        public Comunicado Comunicado { get; set; }
    }
}
