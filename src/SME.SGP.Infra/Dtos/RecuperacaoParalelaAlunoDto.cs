using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaAlunoDto
    {
        public long CodAluno { get; set; }
        public long Id { get; set; }
        public List<ObjetivoRespostaDto> Respostas { get; set; }
        public string TurmaId { get; set; }
        public string TurmaRecuperacaoParalelaId { get; set; }
    }
}