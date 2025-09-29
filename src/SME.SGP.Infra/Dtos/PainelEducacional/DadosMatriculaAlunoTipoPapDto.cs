using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class DadosMatriculaAlunoTipoPapDto
    {
        public int CodigoAluno { get; set; }
        public long CodigoMatricula { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int CodigoTurma { get; set; }
        public TipoPap TipoPap { get; set; }
        public int AnoLetivo { get; set; }
        public bool AbaixoDoLimiteFrequencia { get; set; } = false;

    }
}