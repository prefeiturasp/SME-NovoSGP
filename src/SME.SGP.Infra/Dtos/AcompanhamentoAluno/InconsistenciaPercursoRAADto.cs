using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class InconsistenciaPercursoRAADto
    {
        public string MensagemInconsistenciaPercursoColetivo { get; set; }
        public InconsistenciaPercursoIndividualRAADto InconsistenciaPercursoIndividual { get; set; }
    }
}
