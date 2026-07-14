using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRespostaEncaminhamentoAEE : IRepositorioBase<RespostaEncaminhamentoAEE>
    {
        Task<bool> RemoverPorArquivoId(long arquivoId);
        Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoEncaminhamentoAEEId);
        Task<IEnumerable<RespostaEncaminhamentoAEE>> ObterPorQuestaoEncaminhamentoId(long questaoEncaminhamentoAEEId);
    }
}
