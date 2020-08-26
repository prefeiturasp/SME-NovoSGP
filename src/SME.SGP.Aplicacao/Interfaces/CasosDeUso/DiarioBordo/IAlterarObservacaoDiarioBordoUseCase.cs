using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IAlterarObservacaoDiarioBordoUseCase
    {
        Task<AuditoriaDto> Executar(string observacao, long observacaoId);
    }
}