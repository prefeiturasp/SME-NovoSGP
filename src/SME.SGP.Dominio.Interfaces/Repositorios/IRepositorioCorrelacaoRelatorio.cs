using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCorrelacaoRelatorio : IRepositorioBase<RelatorioCorrelacao>
    {
        Task<RelatorioCorrelacao> ObterPorCodigoCorrelacaoAsync(Guid codigoCorrelacao);
        RelatorioCorrelacao ObterPorCodigoCorrelacao(Guid codigoCorrelacao);
        Task<RelatorioCorrelacao> ObterCorrelacaoJasperPorCodigoAsync(Guid codigoCorrelacao);
        Task<DataCriacaoRelatorioDto> ObterDataCriacaoRelatorio(Guid codigoCorrelacao);
    }
}