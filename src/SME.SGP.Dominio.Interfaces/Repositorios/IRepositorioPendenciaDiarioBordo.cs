using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioPendenciaDiarioBordo : IRepositorioBase<PendenciaDiarioBordo>
    {
        Task Excluir(long aulaId, long componenteCurricularId);
        Task ExcluirPorAulaId(long aulaId);
    }
}
