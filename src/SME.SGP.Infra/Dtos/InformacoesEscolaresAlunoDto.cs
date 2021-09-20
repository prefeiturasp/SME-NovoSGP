using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class InformacoesEscolaresAlunoDto
    {
        public InformacoesEscolaresAlunoDto()
        {
            FrequenciaAlunoPorBimestres = new List<FrequenciaBimestreAlunoDto>();
        }

        public string CodigoAluno { get; set; }
        public int TipoNecessidadeEspecial { get; set; }
        public string DescricaoNecessidadeEspecial { get; set; }
        public int TipoRecurso { get; set; }
        public string DescricaoRecurso { get; set; }
        public string FrequenciaGlobal { get; set; }
        public IEnumerable<FrequenciaBimestreAlunoDto> FrequenciaAlunoPorBimestres { get; set; }
    }
}
