using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDiariosBordoPorDevolutiva
    {
        Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> Executar(long devolutivaId, int anoLetivo);
    }
}
