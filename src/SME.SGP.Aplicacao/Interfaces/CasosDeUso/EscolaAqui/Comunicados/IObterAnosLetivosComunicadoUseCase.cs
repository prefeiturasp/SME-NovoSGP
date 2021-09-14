using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAnosLetivosComunicadoUseCase
    {
        Task<AnoLetivoComunicadoDto> Executar(int anoMinimo);
    }
}
