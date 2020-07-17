using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicadoAluno : IRepositorioBase<ComunicadoAluno>
    {
        Task<IEnumerable<ComunicadoAluno>> ObterPorComunicado(long comunicadoId);

        Task RemoverTodosAlunosComunicado(long comunicadoId);
    }
}
