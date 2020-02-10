using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaAlunoDto
    {
        public long CodAluno { get; set; }
        public long Id { get; set; }
        public List<ObjetivoRespostaDto> Respostas { get; set; }
        public long TurmaId { get; set; }
        public long TurmaRecuperacaoParalelaId { get; set; }
    }
}