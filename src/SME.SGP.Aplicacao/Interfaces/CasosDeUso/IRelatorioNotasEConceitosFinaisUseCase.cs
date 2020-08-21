using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioNotasEConceitosFinaisUseCase
    {
        Task<bool> Executar(FiltroRelatorioNotasEConceitosFinaisDto filtroRelatorioNotasEConceitosFinaisDto);
    }
}