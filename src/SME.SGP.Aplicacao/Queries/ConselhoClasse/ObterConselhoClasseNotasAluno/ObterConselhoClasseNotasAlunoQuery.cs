using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseNotasAlunoQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterConselhoClasseNotasAlunoQuery(long conselhoClasseId, string alunoCodigo)
        {
            ConselhoClasseId = conselhoClasseId;
            AlunoCodigo = alunoCodigo;
        }

        public long ConselhoClasseId { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
