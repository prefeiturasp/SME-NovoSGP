using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaAlunoDto
    {
        public RegistroFrequenciaAlunoDto()
        {
            Aulas = new List<FrequenciaAulaDto>();
        }

        public List<FrequenciaAulaDto> Aulas { get; set; }
        public string CodigoAluno { get; set; }
        public int CodigoSituacaoMatricula { get; set; }
        public string NomeAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
    }
}