using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IAlterarAvisoMuralUseCase
    {
        Task Executar(long avisoId, string mensagem);
    }
}
