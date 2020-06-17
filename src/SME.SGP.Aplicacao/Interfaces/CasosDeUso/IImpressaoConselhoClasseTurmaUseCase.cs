using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IImpressaoConselhoClasseTurmaUseCase
    {
        Task<bool> Executar(FiltroRelatorioConselhoClasseDto filtroRelatorioConselhoClasseDto);
     }
}
