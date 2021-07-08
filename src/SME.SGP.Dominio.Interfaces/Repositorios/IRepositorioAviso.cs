using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAviso : IRepositorioBase<Aviso>
    {
        Task<Aviso> ObterPorClassroomId(long avisoClassroomId);
    }
}
