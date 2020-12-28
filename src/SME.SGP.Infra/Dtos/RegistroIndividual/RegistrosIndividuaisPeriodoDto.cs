using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistrosIndividuaisPeriodoDto
    {
        public IEnumerable<RegistroIndividualDto> RegistrosIndividuais { get; set; }

        public bool PodeRealizarNovoRegistro { get; set; }
    }
}
