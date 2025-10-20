using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre
{
    public class PainelEducacionalNotasVisaoSmeDreRetornoSelectDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string AnoTurma { get; set; }
        public int Bimestre { get; set; }
        public Modalidade Modalidade { get; set; }
        public long QuantidadeAbaixoMediaPortugues { get; set; }
        public long QuantidadeAbaixoMediaMatematica { get; set; }
        public long QuantidadeAbaixoMediaCiencias { get; set; }
        public long QuantidadeAcimaMediaPortugues { get; set; }
        public long QuantidadeAcimaMediaMatematica { get; set; }
        public long QuantidadeAcimaMediaCiencias { get; set; }
    }
}
