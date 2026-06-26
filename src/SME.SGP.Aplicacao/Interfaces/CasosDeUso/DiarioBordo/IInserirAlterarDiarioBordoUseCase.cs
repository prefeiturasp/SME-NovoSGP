using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IInserirAlterarDiarioBordoUseCase : IUseCase<IEnumerable<InserirAlterarDiarioBordoDto>, IEnumerable<AuditoriaDiarioBordoDto>>
    {
    }
}
