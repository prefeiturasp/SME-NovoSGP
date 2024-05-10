using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisAgrupadosPorDreQuery : IRequest<IEnumerable<ComunicadosTotaisPorDreResultado>>
    {
        public int AnoLetivo { get; set; }

        public ObterComunicadosTotaisAgrupadosPorDreQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }
}
