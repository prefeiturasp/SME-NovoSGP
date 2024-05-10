using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AnoLetivoComunicadoDto
    {
        public IEnumerable<int> AnosLetivosHistorico { get; set; }
        public int AnoLetivoAtual { get => DateTime.Now.Year; }
        public bool TemHistorico { get; set; }
    }
}
