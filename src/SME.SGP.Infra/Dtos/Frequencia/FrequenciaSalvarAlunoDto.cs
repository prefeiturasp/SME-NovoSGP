using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequenciaSalvarAlunoDto
    {
        public string CodigoAluno { get; set; }
        public bool Desabilitado { get; set; }
        public IEnumerable<FrequenciaAulaDto> Frequencias { get; set; }
    }
}
