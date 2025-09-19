namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalInformacoesPap : EntidadeBase
    {
        public int Id { get; set; }
        public TipoPap TipoPap { get; set; }
        public int QuantidadeTurmas { get; set; }
        public int QuantidadeEstudantes { get; set; }
        public int QuantidadeEstudantesComFrequenciaInferiorLimite { get; set; }
        public int QuantidadeEstudantesDificuldadeTop1 { get; set; }
        public int QuantidadeEstudantesDificuldadeTop2 { get; set; }
        public int Outras { get; set; }
    }
}
