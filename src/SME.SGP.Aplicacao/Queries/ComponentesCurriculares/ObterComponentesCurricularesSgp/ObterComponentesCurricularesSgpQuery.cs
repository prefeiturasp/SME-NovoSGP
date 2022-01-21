using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesSgpQuery : IRequest<IEnumerable<ComponenteCurricularSgp>>
    {
        public ObterComponentesCurricularesSgpQuery()
        { }
    }

}
