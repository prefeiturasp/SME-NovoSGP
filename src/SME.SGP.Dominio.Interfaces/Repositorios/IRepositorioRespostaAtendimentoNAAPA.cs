using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRespostaAtendimentoNAAPA : IRepositorioBase<RespostaEncaminhamentoNAAPA>
    {
        Task<bool> RemoverPorArquivoId(long arquivoId);
        Task<IEnumerable<RespostaEncaminhamentoNAAPA>> ObterPorQuestaoEncaminhamentoId(long requestQuestaoEncaminhamentoNaapaId);
        Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoEncaminhamentoAEEId);
        Task<IEnumerable<string>> ObterNomesComponenteSecaoComAnexosEmPdf(long encaminhamentoId);
    }
}
