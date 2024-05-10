using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class QuantidadeTotalDiariosPendentesEPreenchidosPorAnoOuTurmaDTO
    {
        public string AnoTurma { get; set; }
        public int QuantidadeTotalDiariosPendentes { get; set; }
        public int QuantidadeTotalDiariosPreenchidos { get; set; }
    }
}
