using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class FechamentoAlunoStatusDto
    {
        public long TurmaId { get; set; }
        public int Ano { get; set; }
        public string AlunoCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Bimestre { get; set; }
        public SituacaoFechamentoAluno Situacao { get; set; }
    }
}