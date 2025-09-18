namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalInformacoesPapDto
    {
        public int Id { get; set; }
        public TipoPap TipoPap { get; set; }   
        public int QuantidadeTurmas { get; set; }
        public int QuantidadeEstudantes { get; set; }
        public int QuantidadeEstudantesDificuldadeTop1 { get; set; }
        public int QuantidadeEstudantesDificuldadeTop2 { get; set; }
        public int OutrasDificuldadesAprendizagem { get; set; }
        public string CodigoDre { get; set; }
        public string NomeDre { get; set; }
        public string CodigoUe { get; set; }
        public string NomeUe { get; set; }
    }
}
