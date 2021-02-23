using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaEncaminhamentoAEE : IRepositorioBase<PendenciaEncaminhamentoAEE>
    {
        Task<PendenciaEncaminhamentoAEE> ObterPorEncaminhamentoAEEId(long encaminhamentoAEEId);
        Task Excluir(long pendenciaId);
    }
}
