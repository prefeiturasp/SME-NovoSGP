using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class FechamentoNotaDuplicado
    {
        public long FechamentoAlunoId { get; set; }
        public long DisciplinaId { get; set; }
        public int Quantidade { get; set; }
        public DateTime PrimeiroRegistro { get; set; }
        public DateTime UltimoRegistro { get; set; }
        public long UltimoId { get; set; }
    }
}
