using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class OrdenarComponentesPorGrupoMatrizAreaConhecimentoQuery : IRequest<IEnumerable<ComponenteCurricularPorTurma>>
    {

        public OrdenarComponentesPorGrupoMatrizAreaConhecimentoQuery(IEnumerable<ComponenteCurricularPorTurma> componentesCurriculares)
        {
            ComponentesCurriculares = componentesCurriculares;
        }
        public IEnumerable<ComponenteCurricularPorTurma> ComponentesCurriculares { get; set; }
    }
}
