namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoNotificacao
    {
        void GeraNovoCodigo(Notificacao notificacao);

        long ObtemNovoCodigo();

        void Salvar(Notificacao notificacao);
    }
}