using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComponenteCurricular
    {
        void SalvarVarias(IEnumerable<ComponenteCurricularDto> componentesCurriculares);
        void AtualizarVarios(IEnumerable<ComponenteCurricularDto> componentesCurriculares);
    }
}