using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarNotificacaoEscolaAquiCommand : IRequest<bool>
    {
        public AlterarNotificacaoEscolaAquiCommand(Comunicado comunicado)
        {
            Comunicado = comunicado;
        }

        public Comunicado Comunicado { get; set; }
    }
}
