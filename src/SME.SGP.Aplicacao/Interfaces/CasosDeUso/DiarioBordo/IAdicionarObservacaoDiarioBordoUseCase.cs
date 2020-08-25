using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IAdicionarObservacaoDiarioBordoUseCase
    {
        Task<AuditoriaDto> Executar(string observacao, long diarioBordoId);
    }
}