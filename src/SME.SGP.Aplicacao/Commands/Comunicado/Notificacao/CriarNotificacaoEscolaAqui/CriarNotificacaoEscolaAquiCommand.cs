using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class CriarNotificacaoEscolaAquiCommand : IRequest<bool>
    {
        public CriarNotificacaoEscolaAquiCommand(Comunicado comunicado)
        {
            this.comunicado = comunicado;
        }

        public Comunicado comunicado { get; set; }
    }
}
