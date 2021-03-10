using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestionario : IRepositorioBase<Questionario>
    {
        Task<IEnumerable<Questao>> ObterQuestoesPorQuestionarioId(long questionarioId);
        Task<long> ObterQuestionarioIdPorTipo(int tipoQuestionario);
    }
}
