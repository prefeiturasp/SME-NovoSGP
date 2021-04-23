using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICalculoFrequenciaTurmaDisciplinaUseCase : IUseCase<MensagemRabbit, bool>
    {
        Task IncluirCalculoFila(CalcularFrequenciaDto calcularFrequenciaDto);
    }
}
