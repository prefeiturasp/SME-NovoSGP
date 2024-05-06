using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase
    {
        Task<OpcoesRespostaFiltroRelatorioMapeamentoEstudanteDto> Executar();
    }
}
