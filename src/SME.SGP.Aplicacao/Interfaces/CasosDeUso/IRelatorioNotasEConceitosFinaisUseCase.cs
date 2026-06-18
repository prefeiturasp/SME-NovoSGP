using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRelatorioNotasEConceitosFinaisUseCase
    {
        Task<bool> Executar(FiltroRelatorioNotasEConceitosFinaisDto filtroRelatorioNotasEConceitosFinaisDto);
    }
}