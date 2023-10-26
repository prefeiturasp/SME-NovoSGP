using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICalculoFrequenciaTurmaDisciplinaUseCase : IRabbitUseCase
    {
        Task IncluirCalculoFila(CalcularFrequenciaDto calcularFrequenciaDto);
    }
}
