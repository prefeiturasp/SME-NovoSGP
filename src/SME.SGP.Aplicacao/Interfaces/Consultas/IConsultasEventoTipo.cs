using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces.Consultas
{
    public interface IConsultasEventoTipo
    {
        IList<EventoTipoDto> Listar(FiltroEventoTipoDto Filtro);

        EventoTipoDto ObtenhaPorId(long id);
    }
}