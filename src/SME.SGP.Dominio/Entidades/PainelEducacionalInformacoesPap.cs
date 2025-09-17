namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalInformacoesPap : EntidadeBase
    {
        public int Id { get; set; }
        public TipoPap TipoPap { get; set; }
        public int QuantidadeTurmas { get; set; }
        public int QuantidadeEstudantes { get; set; }
        public int QuantidadeEstudantesComMenosDe75PorcentoFrequencia { get; set; }
        public int DificuldadeAprendizagem1 { get; set; }
        public int DificuldadeAprendizagem2 { get; set; }
        public int Outras { get; set; }
    }
}
