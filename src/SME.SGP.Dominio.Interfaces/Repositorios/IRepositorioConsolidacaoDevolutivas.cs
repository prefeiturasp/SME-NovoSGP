using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoDevolutivas
    {
        Task<IEnumerable<QuantidadeDevolutivasEstimadasEConfirmadasPorTurmaAnoDto>> ObterDevolutivasEstimadasEConfirmadasAsync(int anoLetivo, Modalidade modalidade, long? dreId, long? ueId);
        
        Task<long> Inserir(ConsolidacaoDevolutivas consolidacao);

        Task LimparConsolidacaoDevolutivasPorAno(int anoLetivo);
        Task<bool> ExisteConsolidacaoDevolutivaTurmaPorAno(int ano);
    }
}
