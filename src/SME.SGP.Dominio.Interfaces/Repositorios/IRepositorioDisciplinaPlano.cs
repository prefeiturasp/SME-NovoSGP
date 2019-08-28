using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDisciplinaPlano : IRepositorioBase<DisciplinaPlano>
    {
        IEnumerable<DisciplinaPlano> ObterDisciplinasPorIdPlano(long idPlano);
    }
}