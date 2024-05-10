using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class CriarNotificacaoEscolaAquiCommand : IRequest<bool>
    {
        public CriarNotificacaoEscolaAquiCommand(Comunicado comunicado)
        {
            Comunicado = comunicado;
        }

        public Comunicado Comunicado { get; set; }
    }
}
