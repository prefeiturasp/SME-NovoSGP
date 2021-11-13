using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeInfantil : IRepositorioBase<AtividadeInfantil>
    {
        Task<AtividadeInfantil> ObterPorAtividadeClassroomId(long atividadeClassroomId);
    }
}