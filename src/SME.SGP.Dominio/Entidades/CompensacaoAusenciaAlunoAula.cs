using System;

namespace SME.SGP.Dominio
{
    public class CompensacaoAusenciaAlunoAula : EntidadeBase
    {
        public long CompensacaoAusenciaAlunoId { get; set; }
        public long RegistroFrequenciaAlunoId { get; set; }
        public int NumeroAula { get; set; }
        public DateTime DataAula { get; set; }
        public bool Excluido { get; set; }

        public void Excluir()
        {
            Excluido = true;
        }

        public void Restaurar()
        {
            Excluido = false;
        }
    }
}
