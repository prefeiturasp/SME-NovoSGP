using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class ConsolidacaoCCNotaDuplicado
    {
        public long ConsolicacaoCCAlunoTurmaId { get; set; }
        public int Bimestre { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long TurmaId { get; set; }
        public int Quantidade { get; set; }
        public DateTime PrimeiroRegistro { get; set; }
        public DateTime UltimoRegistro { get; set; }
        public long UltimoId { get; set; }
    }
}
