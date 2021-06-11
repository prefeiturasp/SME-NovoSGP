using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestaoEncaminhamentoAEE : IRepositorioBase<QuestaoEncaminhamentoAEE>
    {
        Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoAEESecaoId);
        Task<IEnumerable<RespostaQuestaoEncaminhamentoAEEDto>> ObterRespostasEncaminhamento(long encaminhamentoId);
    }
}
