using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoModalidadeCommand : IRequest<bool>
    {
        public InserirComunicadoModalidadeCommand(Comunicado comunicado)
        {
            Comunicado = comunicado;
        }

        public Comunicado Comunicado { get; set; }
    }
}
