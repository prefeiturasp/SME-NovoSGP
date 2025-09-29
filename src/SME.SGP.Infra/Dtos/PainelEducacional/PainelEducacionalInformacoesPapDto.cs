using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalInformacoesPapDto
    {
        public TipoPap TipoPap { get; set; }
        public string TipoPapNome => TipoPap.Name();
        public int QuantidadeTurmas { get; set; }
        public int QuantidadeEstudantes { get; set; }
        public int QuantidadeEstudantesComFrequenciaInferiorLimite { get; set; }
        public int QuantidadeEstudantesDificuldadeTop1 { get; set; }
        public int QuantidadeEstudantesDificuldadeTop2 { get; set; }
        public int OutrasDificuldadesAprendizagem { get; set; }
        public string NomeDificuldadeTop1 { get; set; }
        public string NomeDificuldadeTop2 { get; set; }

    }
}
