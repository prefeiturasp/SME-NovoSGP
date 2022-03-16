using SME.SGP.Dominio;

namespace SME.SGP.Infra
{ 
    public class FechamentoConselhoClasseNotaFinalDto
    {
        public long TurmaId { get; set; }
        public string TurmaAnoNome { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public double? Nota { get; set; }
        public string Conceito { get; set; }
        public bool NotaAcimaMedia { get; set; }
        public int Linha { get; set; }
    }
}
