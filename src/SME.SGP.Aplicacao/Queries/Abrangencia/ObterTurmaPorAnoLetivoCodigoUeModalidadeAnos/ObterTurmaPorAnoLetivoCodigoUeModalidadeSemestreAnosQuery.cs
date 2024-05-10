using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosQuery(int anoLetivo, string codigoUe, Modalidade? modalidade, int semestre, IList<string> anos)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            Semestre = semestre;
            Anos = anos;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public Modalidade? Modalidade { get; set; }
        public int Semestre { get; set; }
        public IList<string> Anos { get; set; }
    }
}

