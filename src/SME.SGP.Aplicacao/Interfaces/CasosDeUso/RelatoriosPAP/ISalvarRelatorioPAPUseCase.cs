using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarRelatorioPAPUseCase
    {
        Task<ResultadoRelatorioPAPDto> Executar(RelatorioPAPDto relatorioPAPDto);
    }
}
