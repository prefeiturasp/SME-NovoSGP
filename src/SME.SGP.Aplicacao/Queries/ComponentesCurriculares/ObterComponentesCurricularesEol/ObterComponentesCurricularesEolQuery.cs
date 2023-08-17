using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEolQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesCurricularesEolQuery() { }

        private static ObterComponentesCurricularesEolQuery _instance;
        public static ObterComponentesCurricularesEolQuery Instance => _instance ??= new();
    }
}
