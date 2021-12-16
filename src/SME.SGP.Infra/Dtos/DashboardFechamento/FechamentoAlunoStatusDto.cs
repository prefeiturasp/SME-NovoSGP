using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FechamentoAlunoStatusDto
    {
        public long TurmaId { get; set; }
        public string Ano { get; set; }
        public string AlunoCodigo { get; set; }
        public string TurmaNome { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Bimestre { get; set; }
        public SituacaoFechamentoAluno Situacao { get; set; }
    }
}