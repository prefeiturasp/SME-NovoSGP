using System.Threading.Tasks;

namespace SME.SGP.Infra.Interface
{
    public interface IServicoArmazenamento
    {
        Task ArmazenarTemporaria();
        Task Armazenar();
        Task Copiar();
        Task Mover();
        Task Excluir();
        Task Obter();
    }
}
