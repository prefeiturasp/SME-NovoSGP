using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirVariosComponentesCurricularesCommand : IRequest<bool>
    {
        public IEnumerable<ComponenteCurricularDto> ComponentesCurriculares { get; set; }

        public InserirVariosComponentesCurricularesCommand(IEnumerable<ComponenteCurricularDto> componentesCurriculares)
        {
            ComponentesCurriculares = componentesCurriculares;
        }
    }
}
