﻿using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoPeriodoFechamento
    {
        void AlterarPeriodosComHierarquiaInferior(PeriodoFechamento fechamento);

        Task<FechamentoDto> ObterPorTipoCalendarioSme(long tipoCalendarioId);

        Task Salvar(FechamentoDto fechamentoDto);
    }
}