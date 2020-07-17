using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaECodigoUeQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesCurricularesPorTurmaECodigoUeQuery(string[] turmaCodigo, string codigoUe)
        {
            TurmaCodigo = turmaCodigo;
            CodigoUe = codigoUe;
        }

        public string[] TurmaCodigo { get; set; }
        public string CodigoUe { get; set; }
    }
}
