using SME.SGP.Infra.Dtos.Relatorios.MapeamentoEstudantes;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase
    {
        Task<OpcoesRespostaFiltroRelatorioMapeamentoEstudanteDto> Executar();
    }
}
