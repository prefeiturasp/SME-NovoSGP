using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class AtualizarVariosComponentesCurricularesCommand : IRequest<bool>
    {
        public IEnumerable<ComponenteCurricularSgp> ComponentesCurriculares { get; set; }

        public AtualizarVariosComponentesCurricularesCommand(IEnumerable<ComponenteCurricularSgp> componentesCurriculares)
        {
            ComponentesCurriculares = componentesCurriculares;
        }
    }
}
