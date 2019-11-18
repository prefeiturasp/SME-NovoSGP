using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAula
    {
        Task<string> Salvar(Aula aula, Usuario usuario);
    }
}