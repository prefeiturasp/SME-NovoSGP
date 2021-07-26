using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAviso : IRepositorioBase<Aviso>
    {
        Task<Aviso> ObterPorClassroomId(long avisoClassroomId);
        Task<IEnumerable<MuralAvisosRetornoDto>> ObterPorAulaId(long aulaId);
    }
}
