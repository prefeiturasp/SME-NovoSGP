using SME.SGP.Dominio.Entidades;
using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCorrelacaoRelatorio : IRepositorioBase<RelatorioCorrelacao>
    {
        RelatorioCorrelacao ObterPorCodigoCorrelacao(Guid codigoCorrelacao);
        RelatorioCorrelacao ObterCorrelacaoJasperPorCodigo(Guid codigoCorrelacao);
    }
}