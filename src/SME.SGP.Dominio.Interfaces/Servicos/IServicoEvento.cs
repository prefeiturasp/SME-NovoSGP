using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoEvento
    {
        Task<long> Salvar(Evento evento);
    }
}