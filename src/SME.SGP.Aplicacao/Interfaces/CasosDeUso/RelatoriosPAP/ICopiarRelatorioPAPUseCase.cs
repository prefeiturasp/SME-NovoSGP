using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface ICopiarRelatorioPAPUseCase
    {
        Task<ResultadoRelatorioPAPDto> Executar(CopiarPapDto copiarPapDto);
    }
}