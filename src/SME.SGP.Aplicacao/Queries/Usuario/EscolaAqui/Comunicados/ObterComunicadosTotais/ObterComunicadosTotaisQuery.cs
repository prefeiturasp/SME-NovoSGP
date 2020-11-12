using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisQuery : IRequest<ComunicadosTotaisSmeResultado>
    {
        public int AnoLetivo { get; set; }

        public ObterComunicadosTotaisQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }
}
