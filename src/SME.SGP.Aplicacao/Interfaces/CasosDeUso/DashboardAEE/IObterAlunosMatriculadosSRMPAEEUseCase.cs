using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosMatriculadosSRMPAEEUseCase : IUseCase<FiltroDashboardAEEDto, IEnumerable<AEEAlunosMatriculadosDto>>
    {
    }
}
