using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosTotaisQuery : IRequest<ComunicadosTotaisResultado>
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }

        public ObterComunicadosTotaisQuery(int anoLetivo, string codigoDre, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

    }
}
