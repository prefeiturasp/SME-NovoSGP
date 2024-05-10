using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroMigracaoFrequenciaDto
    {
        public FiltroMigracaoFrequenciaDto() { }
        public FiltroMigracaoFrequenciaDto(int[] anos)
        {
            Anos = anos;
        }

        public int[] Anos { get; set; }
    }
}
