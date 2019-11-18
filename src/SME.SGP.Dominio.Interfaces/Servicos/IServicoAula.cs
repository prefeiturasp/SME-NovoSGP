using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAula
    {
        Task Salvar(Aula aula, Usuario usuario);
    }
}