using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPorUeQuery : IRequest<IEnumerable<SupervisorEscolasDreDto>>
    {
        public ObterResponsaveisPorUeQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }

        public string CodigoUe { get; set; }
    }
}
