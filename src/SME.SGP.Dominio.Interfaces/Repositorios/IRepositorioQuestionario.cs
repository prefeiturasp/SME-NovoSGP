using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestionario : IRepositorioBase<Questionario>
    {
        Task<IEnumerable<Questao>> ObterQuestoesPorQuestionarioId(long questionarioId);
        Task<long> ObterQuestionarioIdPorTipo(int tipoQuestionario);
        Task<IEnumerable<Questao>> ObterQuestoesPorNomesComponentes(string[] nomesComponentes, TipoQuestionario tipoQuestionario);
    }
}
