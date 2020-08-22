using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioCompensacaoAusenciaUseCase
    {
        Task<bool> Executar(FiltroRelatorioCompensacaoAusenciaDto filtroRelatorioCompensacaoAusenciaDto);
    }
}