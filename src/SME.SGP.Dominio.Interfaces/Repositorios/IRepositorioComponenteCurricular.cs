using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComponenteCurricular
    {
        void SalvarVarias(IEnumerable<ComponenteCurricularDto> componentesCurriculares);
        Task Atualizar(ComponenteCurricularDto componenteCurricular);
    }
}