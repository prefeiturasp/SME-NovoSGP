using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroQuestoesQuestionarioMapeamentoEstudanteDto
    {
        public FiltroQuestoesQuestionarioMapeamentoEstudanteDto()
        {}
        public long QuestionarioId { get; set; }
        public long? MapeamentoEstudanteId { get; set; }
        public string CodigoAluno { get; set; }
        public long? TurmaId { get; set; }
        public int? Bimestre { get; set; }
    }
}
