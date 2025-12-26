using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCorrelacaoInforme : IRepositorioBase<InformeCorrelacao>
    {
        Task<InformeCorrelacao> ObterPorCodigoCorrelacaoAsync(Guid codigoCorrelacao);
        InformeCorrelacao ObterPorCodigoCorrelacao(Guid codigoCorrelacao);
        Task<DataCriacaoInformeDto> ObterDataCriacaoInforme(Guid codigoCorrelacao);
    }
}