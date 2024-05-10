using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoRegistrosPedagogicosPorTurmaDto
    {
        public FiltroConsolidacaoRegistrosPedagogicosPorTurmaDto(string turmaCodigo, int anoLetivo, IEnumerable<ProfessorTitularDisciplinaEol> professorTitularDisciplinaEols)
        {
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
            ProfessorTitularDisciplinaEols = professorTitularDisciplinaEols;
        }

        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public IEnumerable<ProfessorTitularDisciplinaEol> ProfessorTitularDisciplinaEols { get; set; }
    }
}
