using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap
{
    public class ContagemDificuldadeIndicadoresPapPorTipoDto
    {
        public string Abrangencia { get; set; }
        public TipoPap TipoPap { get; set; }
        public int RespostaId { get; set; }
        public string NomeDificuldade { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public int Quantidade { get; set; }
    }
}
