using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesNotasAlunoPorTurmaPeriodoQuery : IRequest<IEnumerable<AvaliacaoNotaAlunoDto>>
    {
        public ObterAtividadesNotasAlunoPorTurmaPeriodoQuery(long turmaId, long periodoEscolarId, string alunoCodigo, string componenteCurricular)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
            AlunoCodigo = alunoCodigo;
            ComponenteCurricular = componenteCurricular;
        }

        public long TurmaId { get; }
        public long PeriodoEscolarId { get; }
        public string AlunoCodigo { get; }
        public string ComponenteCurricular { get; }
    }
}
