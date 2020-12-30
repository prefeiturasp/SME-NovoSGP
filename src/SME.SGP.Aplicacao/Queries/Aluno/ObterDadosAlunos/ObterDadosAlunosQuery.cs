using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosAlunosQuery : IRequest<IEnumerable<AlunoDadosBasicosDto>>
    {
        public ObterDadosAlunosQuery(string turmaCodigo, int anoLetivo, PeriodoEscolar periodoEscolar = null)
        {
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
            PeriodoEscolar = periodoEscolar;
        }

        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
    }
}
