using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExecutarSyncSerapEstudantesProvasUseCase
    {
        Task<bool> Executar();
    }
}
