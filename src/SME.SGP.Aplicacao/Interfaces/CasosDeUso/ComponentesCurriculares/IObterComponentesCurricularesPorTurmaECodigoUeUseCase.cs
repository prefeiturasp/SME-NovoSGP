using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComponentesCurricularesPorTurmaECodigoUeUseCase
    {
        Task<IEnumerable<ComponenteCurricularDto>> Executar(FiltroComponentesCurricularesPorTurmaECodigoUeDto filtroComponentesCurricularesPorTurmaECodigoUeDto);
    }
}
