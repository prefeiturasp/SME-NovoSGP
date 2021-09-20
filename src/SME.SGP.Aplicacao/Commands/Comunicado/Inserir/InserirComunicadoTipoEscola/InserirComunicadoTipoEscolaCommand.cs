using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoTipoEscolaCommand : IRequest<bool>
    {
        public InserirComunicadoTipoEscolaCommand(Comunicado comunicado)
        {
            Comunicado = comunicado;
        }

        public Comunicado Comunicado { get; set; }
    }
}
