using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery : IRequest<IEnumerable<long>>
    {
        public ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery(string alunoCodigo, int bimestre, long turmaId)
        {
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            TurmaId = turmaId;
        }

        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public long TurmaId { get; set; }
    }
}
