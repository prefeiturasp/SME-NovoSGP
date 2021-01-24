using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotaisAdesaoQuery : IRequest<IEnumerable<TotaisAdesaoResultado>>
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }

        public ObterTotaisAdesaoQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }
    }
}
