using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesNotasAlunoPorTurmaPeriodoQuery : IRequest<IEnumerable<AvaliacaoNotaAlunoDto>>
    {
        public ObterAtividadesNotasAlunoPorTurmaPeriodoQuery(long turmaId, long periodoEscolarId, string alunoCodigo)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; }
        public long PeriodoEscolarId { get; }
        public string AlunoCodigo { get; }
    }
}
