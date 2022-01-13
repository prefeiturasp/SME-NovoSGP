using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicadoTurma : IRepositorioBase<ComunicadoTurma>
    {
        Task<bool> RemoverTodasTurmasComunicado(long comunicadoId);

        Task<IEnumerable<ComunicadoTurma>> ObterPorComunicado(long comunicadoId);
    }
}
