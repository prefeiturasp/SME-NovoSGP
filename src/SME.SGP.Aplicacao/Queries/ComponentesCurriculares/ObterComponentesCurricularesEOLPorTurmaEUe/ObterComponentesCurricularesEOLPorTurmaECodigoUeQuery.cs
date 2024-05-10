using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery(string[] codigosDeTurmas, string codigoUe)
        {
            CodigosDeTurmas = codigosDeTurmas;
            CodigoUe = codigoUe;
        }

        public string CodigoUe { get; set; }
        public string[] CodigosDeTurmas { get; set; }
    }
}
