using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestaoEncaminhamentoAEE : IRepositorioBase<QuestaoEncaminhamentoAEE>
    {
        Task<IEnumerable<QuestaoRespostaAeeDto>> ObterListaPorQuestionario(long questionarioId);
        Task<IEnumerable<QuestaoRespostaAeeDto>> ObterListaPorQuestionarioEncaminhamento(long questionarioId, long? encaminhamentoId);
        Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoAEESecaoId);
    }
}
