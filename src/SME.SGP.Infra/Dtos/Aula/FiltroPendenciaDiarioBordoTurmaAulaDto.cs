using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroPendenciaDiarioBordoTurmaAulaDto
    {
        public IEnumerable<ProfessorEComponenteInfantilDto> ProfessoresComponentes { get; set; }
        public AulaComComponenteDto Aula { get; set; }
        public string CodigoTurma { get; set; }
        public string TurmaComModalidade { get; set; }
        public string NomeEscola { get; set; }
    }
}
