using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class InformacoesEscolaresAlunoDto
    {
        public string CodigoAluno { get; set; }
        public int TipoNecessidadeEspecial { get; set; }
        public string DescricaoNecessidadeEspecial { get; set; }
        public int TipoRecurso { get; set; }
        public string DescricaoRecurso { get; set; }
        public double FrequenciaGlobal { get; set; }
        public List<FrequenciaBimestreAlunoDto> FrequenciaAlunoPorBimestres { get; set; }
    }
}
