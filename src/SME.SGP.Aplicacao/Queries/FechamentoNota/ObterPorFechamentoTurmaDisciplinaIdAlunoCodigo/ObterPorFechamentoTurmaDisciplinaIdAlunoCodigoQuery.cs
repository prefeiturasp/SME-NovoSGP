using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentoTurmaDisciplinaIdAlunoCodigoQuery : IRequest<IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        public ObterPorFechamentoTurmaDisciplinaIdAlunoCodigoQuery(long[] ids, string alunoCodigo)
        {
            Ids = ids;
            AlunoCodigo = alunoCodigo;
        }
        public long[] Ids { get; set; }
        public string AlunoCodigo { get; set; }
    }
}