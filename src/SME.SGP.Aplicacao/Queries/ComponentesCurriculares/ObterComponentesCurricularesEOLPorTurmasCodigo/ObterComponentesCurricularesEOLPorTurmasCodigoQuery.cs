using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesCurricularesEOLPorTurmasCodigoQuery(string[] codigosDeTurmas)
        {
            CodigosDeTurmas = codigosDeTurmas;
        }

        public string[] CodigosDeTurmas { get; set; }
    }
}
