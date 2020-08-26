using MediatR;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterUltimaVersaoUseCase
    {
        Task<string> Executar();
    }
}
