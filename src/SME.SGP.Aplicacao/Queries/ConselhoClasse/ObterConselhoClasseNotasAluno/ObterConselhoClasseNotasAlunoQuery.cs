using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseNotasAlunoQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterConselhoClasseNotasAlunoQuery(long conselhoClasseId, string alunoCodigo, long? componenteCurricularId = null)
        {
            ConselhoClasseId = conselhoClasseId;
            AlunoCodigo = alunoCodigo;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long ConselhoClasseId { get; set; }
        public string AlunoCodigo { get; set; }
        public long? ComponenteCurricularId { get; set; }
    }
}
