using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestaoMapeamentoEstudante : IRepositorioBase<QuestaoMapeamentoEstudante>
    {
        Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long mapeamentoEstudanteSecaoId);
        Task<IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>> ObterRespostasMapeamentoEstudante(long mapeamentoEstudanteId);
        Task<long> ObterIdQuestaoPorNomeComponenteQuestionario(long questionarioId, string nomeComponente);
    }
}
