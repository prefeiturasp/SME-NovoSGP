using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IEnviarFilaGravarHistoricoEscolarObservacaoUseCase
    {
        Task<bool> Executar(string codigoAluno, SalvarObservacaoHistoricoEscolarDto salvarObservacaoHistoricoEscolarDto);
    }
}
