using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTodosAlunosNaTurmaQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterTodosAlunosNaTurmaQuery(int codigoTurma, int? codigoAluno = null, int tempoArmazenamentoCache = 720 )
        {
            CodigoTurma = codigoTurma;
            CodigoAluno= codigoAluno;
            TempoArmazenamentoCache = tempoArmazenamentoCache;
        }

        public int CodigoTurma { get; set; }
        public int? CodigoAluno { get; set; }
        public int TempoArmazenamentoCache { get; }
    }
}
