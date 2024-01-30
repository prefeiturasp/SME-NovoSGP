using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestaoRegistroAcaoBuscaAtiva : IRepositorioBase<QuestaoRegistroAcaoBuscaAtiva>
    {
        Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long registroAcaoSecaoId);
        Task<IEnumerable<RespostaQuestaoRegistroAcaoBuscaAtivaDto>> ObterRespostasRegistroAcao(long registroAcaoId);
    }
}
