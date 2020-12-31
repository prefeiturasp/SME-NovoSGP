using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrencia : IRepositorioBase<Ocorrencia>
    {
        Task<IEnumerable<Ocorrencia>> Listar(long diarioBordoId, long usuarioLogadoId);
    }
}
