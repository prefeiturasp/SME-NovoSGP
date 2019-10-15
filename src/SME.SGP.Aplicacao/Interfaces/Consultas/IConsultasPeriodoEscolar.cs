using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Interfaces.Consultas
{
    public interface IConsultasPeriodoEscolar
    {
        PeriodoEscolarListaDto ObterPorTipoCalendario(long codigoTipoCalendario);
    }
}
