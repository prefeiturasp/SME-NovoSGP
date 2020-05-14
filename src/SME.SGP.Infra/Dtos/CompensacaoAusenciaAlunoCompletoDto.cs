using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaAlunoCompletoDto: AlunoAusenteDto
    {
        public double QuantidadeFaltasCompensadas { get; set; }
    }
}
