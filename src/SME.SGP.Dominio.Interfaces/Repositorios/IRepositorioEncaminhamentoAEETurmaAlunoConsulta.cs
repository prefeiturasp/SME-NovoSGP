using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoAEETurmaAlunoConsulta
    {
        Task<IEnumerable<EncaminhamentoAEETurmaAluno>> ObterEncaminhamentoAEETurmaAlunoPorIdAsync(long encaminhamentoAEEId);
    }
}
