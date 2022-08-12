using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class RegistroFrequenciaAluno : EntidadeBase, ICloneable
    {
        public string CodigoAluno { get; set; }
        public int NumeroAula { get; set; }
        public int Valor { get; set; }
        public RegistroFrequencia RegistroFrequencia { get; set; }
        public long RegistroFrequenciaId { get; set; }

        public object Clone()
         => new RegistroFrequenciaAluno()
         {
             AlteradoEm = AlteradoEm,
             AlteradoPor = AlteradoPor,
             AlteradoRF = AlteradoRF,
             CriadoEm = CriadoEm,
             CriadoPor = CriadoPor,
             CriadoRF = CriadoRF,
             CodigoAluno = CodigoAluno,
             NumeroAula = NumeroAula,
             RegistroFrequenciaId = RegistroFrequenciaId
         };
    }
}
