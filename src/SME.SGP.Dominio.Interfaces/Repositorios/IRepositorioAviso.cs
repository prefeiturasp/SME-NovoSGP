using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAviso : IRepositorioBase<Aviso>
    {
        Task<Aviso> ObterPorClassroomId(long avisoClassroomId);
        Task<IEnumerable<MuralAvisosRetornoDto>> ObterPorAulaId(long aulaId);
    }
}
