using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe
{
    public class PainelEducacionalNotasVisaoUeRetornoSelectDto
    {
        public int AnoLetivo { get; set; }
        public string Turma { get; set; }
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
