using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAusenciaAluno : IRepositorioBase<RegistroAusenciaAluno>
    {
        bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId);
        Task SalvarVarios(List<RegistroAusenciaAluno> ausenciasParaAdicionar);
        Task ExcluirVarios(List<long> idsParaExcluir);
    }
}