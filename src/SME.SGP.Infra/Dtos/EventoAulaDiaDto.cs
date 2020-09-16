using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EventoAulaDiaDto
    {
        public EventoAulaDiaDto()
        {
            TemEvento = false;
            TemAula = false;
            TemAulaCJ = false;
            TemAvaliacao = false;
            PossuiPendencia = false;
        }
        public int Dia { get; set; }
        public bool TemEvento { get; set; }
        public bool TemAula { get; set; }
        public bool TemAulaCJ { get; set; }
        public bool TemAvaliacao { get; set; }
        public bool PossuiPendencia { get; set; }
    }
}
