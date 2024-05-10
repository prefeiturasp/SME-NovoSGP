using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao
{
    public interface IObterComponentesCurricularesPorTurmaECodigoUeUseCase
    {
        Task<IEnumerable<ComponenteCurricularDto>> Executar(FiltroComponentesCurricularesPorTurmaECodigoUeDto filtroComponentesCurricularesPorTurmaECodigoUeDto);
    }
}
