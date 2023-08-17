using System.Threading.Tasks;

namespace SME.SGP.Infra.Interfaces
{
    public interface IServicoMetricasMensageria
    {
        Task Publicado(string rota);
        Task Obtido(string rota);
        Task Concluido(string rota);
        Task Erro(string rota);
    }
}
