﻿using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCorrelacaoRelatorio : IRepositorioBase<RelatorioCorrelacao>
    {
        RelatorioCorrelacao ObterPorCodigoCorrelacao(Guid codigoCorrelacao);
        Task<RelatorioCorrelacao> ObterCorrelacaoJasperPorCodigoAsync(Guid codigoCorrelacao);
    }
}