using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAula
    {
        IEnumerable<AulaConsultaDto> Listar(FiltroAulaDto dto);
        AulaConsultaDto BuscarPorId(long id);
    }
}
