using SME.SGP.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaAlunoDto
    {
        public RegistroFrequenciaAlunoDto()
        {
            Aulas = new List<FrequenciaAulaDto>();
        }

        [ListaTemElementos(ErrorMessage = "A lista de aulas é obrigatória")]
        public List<FrequenciaAulaDto> Aulas { get; set; }

        [Required(ErrorMessage = "O código do aluno é obrigatório")]
        public string CodigoAluno { get; set; }

        public SituacaoMatriculaAluno CodigoSituacaoMatricula { get; set; }
        public bool Desabilitado { get; set; }
        public string NomeAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public string SituacaoMatricula { get; set; }
        public bool PermiteAnotacao { get; set; }
        public bool PossuiAnotacao { get; set; }
        public MarcadorFrequenciaDto Marcador { get; set; }
        public IndicativoFrequenciaDto IndicativoFrequencia { get; set; }
    }
}