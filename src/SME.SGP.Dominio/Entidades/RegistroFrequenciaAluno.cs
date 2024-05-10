using System;

namespace SME.SGP.Dominio
{
    public class RegistroFrequenciaAluno : EntidadeBase, ICloneable
    {
        public string CodigoAluno { get; set; }
        public int NumeroAula { get; set; }
        public int Valor { get; set; }
        public RegistroFrequencia RegistroFrequencia { get; set; }
        public Aula Aula { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public long AulaId { get; set; }
        public bool Excluido { get; set; }

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
             RegistroFrequenciaId = RegistroFrequenciaId,
             AulaId = AulaId,
             Valor = Valor
         };
    }
}
