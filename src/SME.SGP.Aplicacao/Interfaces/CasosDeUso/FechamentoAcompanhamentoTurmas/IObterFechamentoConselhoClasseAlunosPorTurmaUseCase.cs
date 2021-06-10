using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterFechamentoConselhoClasseAlunosPorTurmaUseCase : IUseCase<FiltroConselhoClasseConsolidadoTurmaBimestreDto, IEnumerable<ConselhoClasseAlunoDto>>
    { }
}