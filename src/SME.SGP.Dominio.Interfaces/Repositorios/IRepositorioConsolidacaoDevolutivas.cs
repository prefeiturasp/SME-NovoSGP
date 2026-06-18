using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoDevolutivas
    {
        Task Salvar(ConsolidacaoDevolutivas consolidacaoDevolutivas);
    }
}
