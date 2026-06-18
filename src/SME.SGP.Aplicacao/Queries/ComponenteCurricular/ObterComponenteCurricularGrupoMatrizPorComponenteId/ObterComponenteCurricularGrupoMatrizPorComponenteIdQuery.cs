using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteCurricularGrupoMatrizPorComponenteIdQuery : IRequest<ComponenteGrupoMatrizDto>
    {
        public long ComponenteCurricularId { get; set; }
    }
}
