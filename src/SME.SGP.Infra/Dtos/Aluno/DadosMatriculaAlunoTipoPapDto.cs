using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Aluno
{
    public class DadosMatriculaAlunoTipoPapDto
    {
        public int AnoLetivo { get; set; }
        public int CodigoTurma { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int CodigoAluno { get; set; }
        public int ComponenteCurricularId { get; set; }
        public TipoPap TipoPap { get; set; }
    }
}