using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasTipoCalendario
    {
        IEnumerable<TipoCalendarioDto> Listar();
        TipoCalendarioCompletoDto BuscarPorId(long id);
    }
}
