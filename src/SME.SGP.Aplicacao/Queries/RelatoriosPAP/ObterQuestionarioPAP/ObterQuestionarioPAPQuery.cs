using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPAPQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public long QuestionarioId { get; set; } 
        public long? PAPSecaoId { get; set; }
    }
}
