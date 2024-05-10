using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPeriodoEscolarPorTurmaUseCase
    {
        Task<IEnumerable<PeriodoEscolarPorTurmaDto>> Executar(long turmaId);
    }
}