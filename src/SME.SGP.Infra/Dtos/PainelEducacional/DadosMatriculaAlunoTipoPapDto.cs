using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades.MapeamentoPap;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class DadosMatriculaAlunoTipoPapDto
    {
        public int AnoLetivo { get; set; }
        public int CodigoTurma { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int CodigoAluno { get; set; }
        public int CodigoMatricula { get; set; }
        public int ComponenteCurricularId { get; set; }
        //public TipoPap TipoPap => PapComponenteCurricularMap.ObterTipoPapPorComponente(ComponenteCurricularId);
        //public bool AbaixoDoLimiteFrequencia { get; set; } = false;

    }
}