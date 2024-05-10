using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterHistoricoEscolarObservacaoUseCase
    {
        Task<HistoricoEscolarObservacaoDto> Executar(string codigoAluno);
    }
}
