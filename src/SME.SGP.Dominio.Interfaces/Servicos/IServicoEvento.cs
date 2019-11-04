using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoEvento
    {
        Task Salvar(Evento evento, bool dataConfirmada = false);
    }
}