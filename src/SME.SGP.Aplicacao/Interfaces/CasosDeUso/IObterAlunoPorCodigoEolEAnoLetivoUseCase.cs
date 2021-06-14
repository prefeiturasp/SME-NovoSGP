using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAlunoPorCodigoEolEAnoLetivoUseCase
    {
        Task<AlunoReduzidoDto> Executar(string codigoAluno, int anoLetivo);
    }
}
