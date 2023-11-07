using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterTurmasAlunosAusentesUseCase : IUseCase<FiltroObterAlunosAusentesDto, IEnumerable<AlunosAusentesDto>>
    {
    }
}
