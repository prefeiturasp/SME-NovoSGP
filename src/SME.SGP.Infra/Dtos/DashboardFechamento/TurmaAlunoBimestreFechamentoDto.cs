using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class TurmaAlunoBimestreFechamentoDto
    {
        public long TurmaId { get; set; }
        public Modalidade TurmaModalidade { get; set; }
        public int TurmaTipo { get; set; }
        public string Ano { get; set; }
        public string TurmaNome { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public int QuantidadeDisciplinaFechadas { get; set; }
    }
}