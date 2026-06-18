using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

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
