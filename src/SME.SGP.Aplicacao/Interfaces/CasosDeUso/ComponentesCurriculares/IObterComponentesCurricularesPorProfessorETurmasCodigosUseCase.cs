using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IObterComponentesCurricularesPorProfessorETurmasCodigosUseCase : IUseCase<IEnumerable<string>, IEnumerable<DisciplinaNomeDto>>
    {
    }
}
