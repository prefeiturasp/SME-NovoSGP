using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class InconsistenciaPercursoIndividualRAADto
    {
        public string MensagemInsconsistencia { get; set; }
        public IEnumerable<AlunosComInconsistenciaPercursoIndividualRAADto> AlunosComInconsistenciaPercursoIndividualRAA { get; set; }
    }
}
