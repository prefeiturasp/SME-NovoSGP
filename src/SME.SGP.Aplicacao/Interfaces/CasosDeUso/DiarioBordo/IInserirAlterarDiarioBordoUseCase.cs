using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IInserirAlterarDiarioBordoUseCase : IUseCase<IEnumerable<InserirAlterarDiarioBordoDto>, IEnumerable<AuditoriaDiarioBordoDto>>
    {
    }
}
