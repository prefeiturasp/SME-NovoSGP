using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IHistoricoEscolarUseCase
    {
        Task<bool> Executar(FiltroHistoricoEscolarDto filtroHistoricoEscolarDto);
    }
}
