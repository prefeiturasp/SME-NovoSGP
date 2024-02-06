using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.Questionario
{
    public class SecaoQuestoesDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public long QuestionarioId { get; set; }
        public string NomeComponente { get; set; }
        public int Ordem { get; set; }
        public TipoQuestionario TipoQuestionario { get; set; }
        public int[] ModalidadesCodigo { get; set; }
        public IEnumerable<QuestaoDto> Questoes { get; set; }
}
}
