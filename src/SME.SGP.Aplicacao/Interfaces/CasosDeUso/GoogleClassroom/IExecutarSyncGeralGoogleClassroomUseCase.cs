using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExecutarSyncGeralGoogleClassroomUseCase
    {
        Task<bool> Executar();
    }
}
