using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioProfessorTurma : IRepositorioBase<Ciclo>
    {
        IEnumerable<CicloDto> ObterTurmasPorProfessor(string codigoRf);
    }
}