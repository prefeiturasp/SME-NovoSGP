using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        IEnumerable<AulaDto> ObterAulas(long tipoCalendarioId, string turmaId, string ueId);
    }
}