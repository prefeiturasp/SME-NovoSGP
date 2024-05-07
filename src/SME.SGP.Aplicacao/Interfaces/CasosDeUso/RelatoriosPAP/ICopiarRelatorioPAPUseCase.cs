using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface ICopiarRelatorioPAPUseCase
    {
        Task<bool> Executar(CopiarPapDto copiarPapDto);
    }
}