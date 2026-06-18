using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICopiarRelatorioPAPUseCase
    {
        Task<bool> Executar(CopiarPapDto copiarPapDto);
    }
}