using MediatR;

namespace SME.SGP.Aplicacao
{
    public class EnviarFilaNotificacaoCriacaoPlanoAEECommand : IRequest<bool>
    {
        public EnviarFilaNotificacaoCriacaoPlanoAEECommand(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; }
    }
}
