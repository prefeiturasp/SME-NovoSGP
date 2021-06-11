using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutaSyncGsaGoogleClassroomUseCase
    {
        Task<bool> Executar();
    }
}
