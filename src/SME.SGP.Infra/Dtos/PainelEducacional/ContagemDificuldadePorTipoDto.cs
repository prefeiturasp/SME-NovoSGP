using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class ContagemDificuldadePorTipoDto
    {
        public TipoPap TipoPap { get; set; }
        public int QuantidadeEstudantesDificuldadeTop1 { get; set; }
        public int QuantidadeEstudantesDificuldadeTop2 { get; set; }
        public int OutrasDificuldadesAprendizagem { get; set; }
        public string CodigoDre { get; set; }
        public string NomeDre { get; set; }
        public string CodigoUe { get; set; }
        public string NomeUe { get; set; }
        public string NomeDificuldadeTop1 { get; set; }
        public string NomeDificuldadeTop2 { get; set; }
    }
}