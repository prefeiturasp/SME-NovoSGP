using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IComandosNotificacao
    {
        void MarcarComoLida(long notificacaoId);

        void Salvar(NotificacaoDto notificacaoDto);
    }
}