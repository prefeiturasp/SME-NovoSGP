using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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