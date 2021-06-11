using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterBimestreAtualPorTurmaIdUseCase
    {
        Task<BimestreDto> Executar(long turmaId);
    }
}