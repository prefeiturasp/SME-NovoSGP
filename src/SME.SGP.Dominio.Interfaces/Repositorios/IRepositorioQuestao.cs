using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestao : IRepositorioBase<Questao>
    {
        Task<bool> VerificaObrigatoriedade(long questaoId);
        Task<IEnumerable<Questao>> ObterQuestoesPorIds(long[] questaoIds);
        Task<Questao> ObterPorNomeComponente(string nomeComponente);
        Task<long?> ObterIdQuestaoPorTipoQuestaoParaQuestionario(long idQuestionario, TipoQuestao tipo);
    }
}
