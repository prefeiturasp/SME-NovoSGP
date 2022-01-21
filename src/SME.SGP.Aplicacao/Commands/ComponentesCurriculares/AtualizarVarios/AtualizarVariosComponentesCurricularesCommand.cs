using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AtualizarVariosComponentesCurricularesCommand : IRequest<bool>
    {
        public IEnumerable<ComponenteCurricularDto> ComponentesCurriculares { get; set; }

        public AtualizarVariosComponentesCurricularesCommand(IEnumerable<ComponenteCurricularDto> componentesCurriculares)
        {
            ComponentesCurriculares = componentesCurriculares;
        }
    }
}
