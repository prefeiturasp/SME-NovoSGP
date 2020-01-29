using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoNotificacao
    {
        Task ExcluirFisicamenteAsync(long[] ids);

        void GeraNovoCodigo(Notificacao notificacao);

        long ObtemNovoCodigo();

        void Salvar(Notificacao notificacao);
    }
}