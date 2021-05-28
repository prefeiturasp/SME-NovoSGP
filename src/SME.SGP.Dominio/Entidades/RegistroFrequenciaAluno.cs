using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class RegistroFrequenciaAluno : EntidadeBase
    {
        public string CodigoAluno { get; set; }
        public int NumeroAula { get; set; }
        public int Valor { get; set; }
        public RegistroFrequencia RegistroFrequencia { get; set; }
        public long RegistroFrequenciaId { get; set; }
    }
}
