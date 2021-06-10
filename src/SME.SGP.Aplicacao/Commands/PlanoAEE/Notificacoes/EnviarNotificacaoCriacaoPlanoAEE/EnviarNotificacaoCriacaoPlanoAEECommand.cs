using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoCriacaoPlanoAEECommand : IRequest<bool>
    {
        public EnviarNotificacaoCriacaoPlanoAEECommand(long planoAEEId, Usuario usuario)
        {
            PlanoAEEId = planoAEEId;
            Usuario = usuario;
        }

        public long PlanoAEEId { get; }
        public Usuario Usuario { get; }
    }
}
