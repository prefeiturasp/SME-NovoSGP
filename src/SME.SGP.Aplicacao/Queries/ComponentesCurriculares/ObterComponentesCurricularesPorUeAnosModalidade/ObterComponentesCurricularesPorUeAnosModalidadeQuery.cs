using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUeAnosModalidadeQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesPorUeAnosModalidadeQuery(long[] turmaCodigos, int anoLetivo, Modalidade modalidade, string[] anos)
        {
            TurmaCodigos = turmaCodigos;
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            Anos = anos;
        }

        public long[] TurmaCodigos { get; set; }
        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string[] Anos { get; set; }
    }
}
