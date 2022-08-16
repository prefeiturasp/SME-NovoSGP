using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseNotasAlunoQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterConselhoClasseNotasAlunoQuery(long conselhoClasseId, string alunoCodigo, int bimestre)
        {
            ConselhoClasseId = conselhoClasseId;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
        }

        public long ConselhoClasseId { get; }
        public string AlunoCodigo { get; }
        public int Bimestre { get; }
    }
}
