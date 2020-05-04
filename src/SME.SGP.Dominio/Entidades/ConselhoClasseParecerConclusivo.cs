using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class ConselhoClasseParecerConclusivo : EntidadeBase
    {
        public string Nome { get; set; }
        public bool Aprovado { get; set; }
        public bool Frequencia { get; set; }
        public bool Nota { get; set; }
        public bool Conselho { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime? FimVigencia { get; set; }
    }
}
