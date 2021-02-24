using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExecutaNotificacaoGoogleClassroomUseCase
    {
        Task<bool> Executar();
    }
}
