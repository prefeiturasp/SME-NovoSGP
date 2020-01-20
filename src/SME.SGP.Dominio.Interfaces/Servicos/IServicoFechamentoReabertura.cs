using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoReabertura
    {
        Task Salvar(FechamentoReabertura fechamentoReabertura);
    }
}