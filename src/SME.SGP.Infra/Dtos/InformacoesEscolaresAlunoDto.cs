using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class InformacoesEscolaresAlunoDto
    {
        public string CodigoAluno { get; set; }

        public string indicativoDeficiencia { get; set; }

        public string RecursosUtilizados { get; set; }

        public string FrequenciaGlobal { get; set; }

        public List<FrequenciaBimestreAlunoDto> FrequenciaAlunoPorBimestres { get; set; }

    }
}
