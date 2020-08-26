using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IUseCase<in TParameter, TResponse>
    {
        Task<TResponse> Executar(TParameter param);
    }
}
