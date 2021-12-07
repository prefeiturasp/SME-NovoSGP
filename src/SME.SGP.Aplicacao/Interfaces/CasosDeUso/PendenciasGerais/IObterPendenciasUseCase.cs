using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPendenciasUseCase 
    {
        Task<PaginacaoResultadoDto<PendenciaDto>> Executar(string turmaId, int? tipoPendencia, string tituloPendencia);
    }
}
