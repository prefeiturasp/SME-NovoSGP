using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class ContagemDificuldadePorTipoDto
    {
        public TipoPap TipoPap { get; set; }
        public int DificuldadeAprendizagem1 { get; set; }
        public int DificuldadeAprendizagem2 { get; set; }
        public int OutrasDificuldadesAprendizagem { get; set; }
    }
}