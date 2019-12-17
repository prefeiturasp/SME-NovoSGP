using System;

namespace SME.SGP.Dominio
{
    public class RegistroAusenciaAluno : EntidadeBase, ICloneable
    {
        public RegistroAusenciaAluno(string codigoAluno, int numeroAula)
        {
            CodigoAluno = codigoAluno;
            NumeroAula = numeroAula;
        }

        protected RegistroAusenciaAluno()
        {
        }

        public string CodigoAluno { get; set; }
        public bool Migrado { get; set; }
        public int NumeroAula { get; set; }
        public RegistroFrequencia RegistroFrequencia { get; set; }
        public long RegistroFrequenciaId { get; set; }

        public object Clone()
         => new RegistroAusenciaAluno()
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