using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery(int anoLetivo, string codigoUe, int[] modalidades, int semestre, string[] anos)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            Modalidades = modalidades;
            Semestre = semestre;
            Anos = anos;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public string[] Anos { get; set; }
    }
}
