using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroPendenciaDiarioBordoTurmaAulaDto
    {
        public List<AulaProfessorComponenteDto> AulasProfessoresComponentesCurriculares { get; set; }
        public string CodigoTurma { get; set; }
        public string TurmaComModalidade { get; set; }
        public string NomeEscola { get; set; }

        public FiltroPendenciaDiarioBordoTurmaAulaDto() { }
    }
}
