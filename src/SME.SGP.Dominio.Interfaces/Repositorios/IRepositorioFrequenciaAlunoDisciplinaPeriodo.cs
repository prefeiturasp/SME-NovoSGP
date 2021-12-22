using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaAlunoDisciplinaPeriodo : IRepositorioBase<FrequenciaAluno>
    {
        Task SalvarVariosAsync(IEnumerable<FrequenciaAluno> entidades);
        Task RemoverVariosAsync(long[] idsParaRemover);
        Task RemoverFrequenciaGeralAlunos(string[] alunos, string turmaCodigo, long periodoEscolarId);
        Task RemoverFrequenciasDuplicadas(string[] alunos, string turmaCodigo, long periodoEscolarId);
    }
}