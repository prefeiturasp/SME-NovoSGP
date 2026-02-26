using SME.SGP.Dominio.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio
{
    public interface IObterSolicitacaoRelatorioUseCase
    {
        Task<bool> Executar(FiltroSolicitacaoRelatorioDto filtroRelatorio);
    }
}
