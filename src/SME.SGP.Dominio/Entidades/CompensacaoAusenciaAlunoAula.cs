using System;

namespace SME.SGP.Dominio.Entidades
{
    public class CompensacaoAusenciaAlunoAula : EntidadeBase
    {
        public long CompensacaoAusenciaAlunoId { get; set; }
        public long RegistroFrequenciaAlunoId { get; set; }
        public int NumeroAula { get; set; }
        public DateTime DataAula { get; set; }
        public bool Excluido { get; set; }
    }
}
