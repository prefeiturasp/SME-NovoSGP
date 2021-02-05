using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class PlanoAEEAlunoTurmaDto
    {
        public long Id { get; set; }
        public string AlunoNumero { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public string TurmaCodigo { get; set; }
        public string TurmaNome { get; set; }
        public int TurmaAno { get; set; }
        public Modalidade TurmaModalidade { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
    }
}
