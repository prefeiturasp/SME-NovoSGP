using SME.SGP.Dominio.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio
{
    public interface IObterSolicitacaoRelatorioUseCase
    {
        Task<long> Executar(FiltroSolicitacaoRelatorioDto filtroRelatorio);
    }
}
