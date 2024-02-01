using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Informes;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterInformesPorFiltroUseCase
    {
        Task<PaginacaoResultadoDto<InformeResumoDto>> Executar(InformeFiltroDto filtro);
    }
}
