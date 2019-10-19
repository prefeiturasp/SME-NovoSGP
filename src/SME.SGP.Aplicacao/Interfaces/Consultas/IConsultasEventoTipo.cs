using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Interfaces.Consultas
{
    public interface IConsultasEventoTipo
    {
        EventoTipoDto ObtenhaPorCodigo(long codigo);
        IList<EventoTipoDto> Listar(FiltroEventoTipoDto Filtro);
    }
}
