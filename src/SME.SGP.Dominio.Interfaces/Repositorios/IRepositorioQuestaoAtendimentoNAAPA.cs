using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestaoAtendimentoNAAPA : IRepositorioBase<QuestaoEncaminhamentoNAAPA>
    {
        Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoNAAPASecaoId);
        Task<IEnumerable<RespostaQuestaoAtendimentoNAAPADto>> ObterRespostasEncaminhamento(long encaminhamentoId);
        Task<IEnumerable<PrioridadeAtendimentoNAAPADto>> ObterPrioridadeEncaminhamento();
        Task<IEnumerable<RespostaQuestaoAtendimentoNAAPADto>> ObterRespostasItinerarioEncaminhamento(long encaminhamentoSecaoId);
        Task<QuestaoEncaminhamentoNAAPA> ObterQuestaoEnderecoResidencialPorEncaminhamentoId(long encaminhamentoNAAPAId);
        Task<QuestaoEncaminhamentoNAAPA> ObterQuestaoTurmasProgramaPorEncaminhamentoId(long encaminhamentoNAAPAId);
        Task<long> ObterIdQuestaoPorNomeComponenteQuestionario(long questionarioId, string nomeComponente);
    }
}
