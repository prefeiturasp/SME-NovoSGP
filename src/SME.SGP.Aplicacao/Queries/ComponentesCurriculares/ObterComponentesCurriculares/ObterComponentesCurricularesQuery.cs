using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesCurricularesQuery()
        { }

        private static ObterComponentesCurricularesQuery _instance;
        public static ObterComponentesCurricularesQuery Instance => _instance ??= new();
    }

}
